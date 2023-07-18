﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Utilities;
using Avalonia.VisualTree;

namespace BilibiliClient.Controls;

/// <summary>
/// Positions child elements in sequential position from left to right, 
/// breaking content to the next line at the edge of the containing box. 
/// Subsequent ordering happens sequentially from top to bottom or from right to left, 
/// depending on the value of the <see cref="Orientation"/> property.
/// </summary>
public class VirtualizingWrapPanel : VirtualizingPanel
{
    /// <summary>
    /// Defines the <see cref="Orientation"/> property.
    /// </summary>
    public static readonly StyledProperty<Orientation> OrientationProperty =
        StackPanel.OrientationProperty.AddOwner<VirtualizingWrapPanel>();

    /// <summary>
    /// Defines the <see cref="ItemWidth"/> property.
    /// </summary>
    public static readonly StyledProperty<double> ItemWidthProperty =
        AvaloniaProperty.Register<VirtualizingWrapPanel, double>(nameof(ItemWidth), double.NaN);

    /// <summary>
    /// Defines the <see cref="ItemHeight"/> property.
    /// </summary>
    public static readonly StyledProperty<double> ItemHeightProperty =
        AvaloniaProperty.Register<VirtualizingWrapPanel, double>(nameof(ItemHeight), double.NaN);

    private static readonly AttachedProperty<bool> ItemIsOwnContainerProperty =
        AvaloniaProperty.RegisterAttached<VirtualizingWrapPanel, Control, bool>("ItemIsOwnContainer");

    private static readonly Rect s_invalidViewport = new(double.PositiveInfinity, double.PositiveInfinity, 0, 0);
    private readonly Action<Control, int> _recycleElement;
    private readonly Action<Control> _recycleElementOnItemRemoved;
    private readonly Action<Control, int, int> _updateElementIndex;
    private int _scrollToIndex = -1;
    private Control? _scrollToElement;
    private bool _isInLayout;
    private bool _isWaitingForViewportUpdate;
    private UVSize _lastEstimatedElementSizeUV = new UVSize(Orientation.Horizontal, 25, 25);
    private RealizedWrappedElements? _measureElements;
    private RealizedWrappedElements? _realizedElements;
    private ScrollViewer? _scrollViewer;
    private Rect _viewport = s_invalidViewport;
    private Dictionary<object, Stack<Control>>? _recyclePool;
    private Control? _unrealizedFocusedElement;
    private int _unrealizedFocusedIndex = -1;
    private Control? _focusedElement;
    private int _focusedIndex = -1;
    private static readonly object s_itemIsItsOwnContainer = new object();


    private static readonly AttachedProperty<object?> RecycleKeyProperty =
        AvaloniaProperty.RegisterAttached<VirtualizingStackPanel, Control, object?>("RecycleKey");

    static VirtualizingWrapPanel()
    {
        OrientationProperty.OverrideDefaultValue(typeof(VirtualizingWrapPanel), Orientation.Horizontal);
    }

    public VirtualizingWrapPanel()
    {
        _recycleElement = RecycleElement;
        _recycleElementOnItemRemoved = RecycleElementOnItemRemoved;
        _updateElementIndex = UpdateElementIndex;
        EffectiveViewportChanged += OnEffectiveViewportChanged;
    }

    /// <summary>
    /// Gets or sets the axis along which items are laid out.
    /// </summary>
    /// <value>
    /// One of the enumeration values that specifies the axis along which items are laid out.
    /// The default is Vertical.
    /// </value>
    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Gets or sets the width of all items in the WrapPanel.
    /// </summary>
    public double ItemWidth
    {
        get { return GetValue(ItemWidthProperty); }
        set { SetValue(ItemWidthProperty, value); }
    }

    /// <summary>
    /// Gets or sets the height of all items in the WrapPanel.
    /// </summary>
    public double ItemHeight
    {
        get { return GetValue(ItemHeightProperty); }
        set { SetValue(ItemHeightProperty, value); }
    }

    /// <summary>
    /// Gets the index of the first realized element, or -1 if no elements are realized.
    /// </summary>
    public int FirstRealizedIndex => _realizedElements?.FirstIndex ?? -1;

    /// <summary>
    /// Gets the index of the last realized element, or -1 if no elements are realized.
    /// </summary>
    public int LastRealizedIndex => _realizedElements?.LastIndex ?? -1;

    protected override Size MeasureOverride(Size availableSize)
    {
        var items = Items;

        if (items.Count == 0)
            return default;

        // If we're bringing an item into view, ignore any layout passes until we receive a new
        // effective viewport.
        if (_isWaitingForViewportUpdate)
            return DesiredSize;

        _isInLayout = true;

        try
        {
            var orientation = Orientation;

            _realizedElements ??= new();
            _measureElements ??= new();

            // We handle horizontal and vertical layouts here so X and Y are abstracted to:
            // - Horizontal layouts: U = horizontal, V = vertical
            // - Vertical layouts: U = vertical, V = horizontal
            var viewport = CalculateMeasureViewport(items);

            // If the viewport is disjunct then we can recycle everything.
            if (viewport.viewportIsDisjunct)
                _realizedElements.RecycleAllElements(_recycleElement, orientation);

            // Do the measure, creating/recycling elements as necessary to fill the viewport. Don't
            // write to _realizedElements yet, only _measureElements.
            RealizeElements(items, availableSize, ref viewport);

            // Now swap the measureElements and realizedElements collection.
            (_measureElements, _realizedElements) = (_realizedElements, _measureElements);
            _measureElements.ResetForReuse(Orientation);

            return CalculateDesiredSize(orientation, items.Count, viewport);
        }
        finally
        {
            _isInLayout = false;
        }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (_realizedElements is null)
            return default;

        _isInLayout = true;

        try
        {
            var orientation = Orientation;

            for (var i = 0; i < _realizedElements.Count; ++i)
            {
                var e = _realizedElements.Elements[i];

                if (e is not null)
                {
                    var sizeUV = _realizedElements.SizeUV;
                    var positionUV = _realizedElements.PositionsUV[i];
                    var rect = new Rect(positionUV.Width, positionUV.Height, sizeUV.Width, sizeUV.Height);
                    e.Arrange(rect);
                    _scrollViewer?.RegisterAnchorCandidate(e);
                }
            }

            return finalSize;
        }
        finally
        {
            _isInLayout = false;
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _scrollViewer = this.FindAncestorOfType<ScrollViewer>();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        _scrollViewer = null;
    }

    protected override void OnItemsChanged(IReadOnlyList<object?> items, NotifyCollectionChangedEventArgs e)
    {
        InvalidateMeasure();

        if (_realizedElements is null)
            return;

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                _realizedElements.ItemsInserted(e.NewStartingIndex, e.NewItems!.Count, _updateElementIndex);
                break;
            case NotifyCollectionChangedAction.Remove:
                _realizedElements.ItemsRemoved(e.OldStartingIndex, e.OldItems!.Count, _updateElementIndex,
                    _recycleElementOnItemRemoved);
                break;
            case NotifyCollectionChangedAction.Replace:
            case NotifyCollectionChangedAction.Move:
                _realizedElements.ItemsRemoved(e.OldStartingIndex, e.OldItems!.Count, _updateElementIndex,
                    _recycleElementOnItemRemoved);
                _realizedElements.ItemsInserted(e.NewStartingIndex, e.NewItems!.Count, _updateElementIndex);
                break;
            case NotifyCollectionChangedAction.Reset:
                _realizedElements.ItemsReset(_recycleElementOnItemRemoved, Orientation);
                break;
        }
    }

    protected override IInputElement? GetControl(NavigationDirection direction, IInputElement? from, bool wrap)
    {
        var count = Items.Count;

        if (count == 0 || from is not Control fromControl)
            return null;

        var horiz = Orientation == Orientation.Horizontal;
        var fromIndex = from != null ? IndexFromContainer(fromControl) : -1;
        var toIndex = fromIndex;

        switch (direction)
        {
            case NavigationDirection.First:
                toIndex = 0;
                break;
            case NavigationDirection.Last:
                toIndex = count - 1;
                break;
            case NavigationDirection.Next:
                ++toIndex;
                break;
            case NavigationDirection.Previous:
                --toIndex;
                break;
            case NavigationDirection.Left:
                if (horiz)
                    --toIndex;
                break;
            case NavigationDirection.Right:
                if (horiz)
                    ++toIndex;
                break;
            case NavigationDirection.Up:
                if (!horiz)
                    --toIndex;
                break;
            case NavigationDirection.Down:
                if (!horiz)
                    ++toIndex;
                break;
            default:
                return null;
        }

        if (fromIndex == toIndex)
            return from;

        if (wrap)
        {
            if (toIndex < 0)
                toIndex = count - 1;
            else if (toIndex >= count)
                toIndex = 0;
        }

        return ScrollIntoView(toIndex);
    }

    protected override IEnumerable<Control> GetRealizedContainers()
    {
        return _realizedElements?.Elements.Where(x => x is not null)!;
    }

    protected override Control? ContainerFromIndex(int index)
    {
        if (index < 0 || index >= Items.Count)
            return null;
        if (_scrollToIndex == index)
            return _scrollToElement;
        if (_focusedIndex == index)
            return _focusedElement;
        if (GetRealizedElement(index) is { } realized)
            return realized;
        if (Items[index] is Control c && c.GetValue(RecycleKeyProperty) == s_itemIsItsOwnContainer)
            return c;
        return null;
    }

    protected override int IndexFromContainer(Control container) => _realizedElements?.GetIndex(container) ?? -1;

    protected override Control? ScrollIntoView(int index)
    {
        var items = Items;

        if (_isInLayout || index < 0 || index >= items.Count || _realizedElements is null)
            return null;

        if (GetRealizedElement(index) is { } element)
        {
            element.BringIntoView();
            return element;
        }
        else if (this.GetVisualRoot() is ILayoutRoot root)
        {
            // Create and measure the element to be brought into view. Store it in a field so that
            // it can be re-used in the layout pass.
            double itemWidth = ItemWidth;
            double itemHeight = ItemHeight;
            bool isItemWidthSet = !double.IsNaN(itemWidth);
            bool isItemHeightSet = !double.IsNaN(itemHeight);
            var size = new Size(isItemWidthSet ? itemWidth : double.PositiveInfinity,
                isItemHeightSet ? itemHeight : double.PositiveInfinity);
            _scrollToElement = GetOrCreateElement(items, index);
            _scrollToElement.Measure(size);
            _scrollToIndex = index;

            var viewport = _viewport != s_invalidViewport ? _viewport : EstimateViewport();
            var viewportEnd = Orientation == Orientation.Horizontal
                ? new UVSize(Orientation, viewport.Right, viewport.Bottom)
                : new UVSize(Orientation, viewport.Bottom, viewport.Right);

            // Get the expected position of the element and put it in place.
            var anchorUV =
                _realizedElements.GetOrEstimateElementUV(index, ref _lastEstimatedElementSizeUV, viewportEnd);
            size = new Size(isItemWidthSet ? itemWidth : _scrollToElement.DesiredSize.Width,
                isItemHeightSet ? itemHeight : _scrollToElement.DesiredSize.Height);
            var rect = new Rect(anchorUV.Width, anchorUV.Height, size.Width, size.Height);
            _scrollToElement.Arrange(rect);

            // If the item being brought into view was added since the last layout pass then
            // our bounds won't be updated, so any containing scroll viewers will not have an
            // updated extent. Do a layout pass to ensure that the containing scroll viewers
            // will be able to scroll the new item into view.
            if (!Bounds.Contains(rect) && !_viewport.Contains(rect))
            {
                _isWaitingForViewportUpdate = true;
                // root.LayoutManager.ExecuteLayoutPass();
                UpdateLayout();
                _isWaitingForViewportUpdate = false;
            }

            // Try to bring the item into view.
            _scrollToElement.BringIntoView();

            // If the viewport does not contain the item to scroll to, set _isWaitingForViewportUpdate:
            // this should cause the following chain of events:
            // - Measure is first done with the old viewport (which will be a no-op, see MeasureOverride)
            // - The viewport is then updated by the layout system which invalidates our measure
            // - Measure is then done with the new viewport.
            _isWaitingForViewportUpdate = !_viewport.Contains(rect);
            // root.LayoutManager.ExecuteLayoutPass();
            UpdateLayout();
            // If for some reason the layout system didn't give us a new viewport during the layout, we
            // need to do another layout pass as the one that took place was a no-op.
            if (_isWaitingForViewportUpdate)
            {
                _isWaitingForViewportUpdate = false;
                InvalidateMeasure();
                // root.LayoutManager.ExecuteLayoutPass();
                UpdateLayout();
            }

            var result = _scrollToElement;
            _scrollToElement = null;
            _scrollToIndex = -1;
            return result;
        }

        return null;
    }

    private UVSize EstimateElementSizeUV()
    {
        double itemWidth = ItemWidth;
        double itemHeight = ItemHeight;
        bool isItemWidthSet = !double.IsNaN(itemWidth);
        bool isItemHeightSet = !double.IsNaN(itemHeight);

        var estimatedSize = new UVSize(Orientation,
            isItemWidthSet ? itemWidth : _lastEstimatedElementSizeUV.Width,
            isItemHeightSet ? itemHeight : _lastEstimatedElementSizeUV.Height);

        if ((isItemWidthSet && isItemHeightSet) || _realizedElements is null)
            return estimatedSize;

        var result = _realizedElements.EstimateElementSize(Orientation);
        if (result != null)
        {
            estimatedSize = result.Value;
            estimatedSize.Width = isItemWidthSet ? itemWidth : estimatedSize.Width;
            estimatedSize.Height = isItemHeightSet ? itemHeight : estimatedSize.Height;
        }

        return estimatedSize;
    }

    internal IReadOnlyList<Control?> GetRealizedElements()
    {
        return _realizedElements?.Elements ?? Array.Empty<Control>();
    }

    private MeasureViewport CalculateMeasureViewport(IReadOnlyList<object?> items)
    {
        Debug.Assert(_realizedElements is not null);

        // If the control has not yet been laid out then the effective viewport won't have been set.
        // Try to work it out from an ancestor control.
        var viewport = _viewport != s_invalidViewport ? _viewport : EstimateViewport();

        // Get the viewport in the orientation direction.
        var viewportStart = new UVSize(Orientation, viewport.X, viewport.Y);
        var viewportEnd = new UVSize(Orientation, viewport.Right, viewport.Bottom);

        // Get or estimate the anchor element from which to start realization.
        var itemCount = items?.Count ?? 0;
        _lastEstimatedElementSizeUV.Orientation = Orientation;
        var (anchorIndex, anchorU) = _realizedElements.GetOrEstimateAnchorElementForViewport(
            viewportStart,
            viewportEnd,
            itemCount,
            ref _lastEstimatedElementSizeUV);

        // Check if the anchor element is not within the currently realized elements.
        var disjunct = anchorIndex < _realizedElements.FirstIndex ||
                       anchorIndex > _realizedElements.LastIndex;

        return new MeasureViewport
        {
            anchorIndex = anchorIndex,
            anchorUV = anchorU,
            viewportUVStart = viewportStart,
            viewportUVEnd = viewportEnd,
            viewportIsDisjunct = disjunct,
        };
    }

    private Size CalculateDesiredSize(Orientation orientation, int itemCount, in MeasureViewport viewport)
    {
        var sizeUV = new UVSize(orientation);
        var estimatedSize = EstimateElementSizeUV();

        if (!double.IsNaN(ItemWidth) && !double.IsNaN(ItemHeight))
        {
            // Since ItemWidth and ItemHeight are set, we simply compute the actual size
            var uLength = viewport.viewportUVEnd.U;
            var estimatedItemsPerU = (int)(uLength / estimatedSize.U);
            var estimatedULanes = Math.Ceiling((double)itemCount / estimatedItemsPerU);
            sizeUV.U = Math.Max(estimatedItemsPerU * estimatedSize.U, estimatedSize.U);
            sizeUV.V = estimatedULanes * estimatedSize.V;

            if (double.IsInfinity(sizeUV.V))
                sizeUV.V = estimatedSize.V;
        }
        else if (viewport.lastIndex >= 0)
        {
            var remaining = itemCount - viewport.lastIndex - 1;
            sizeUV = viewport.realizedEndUV;
            var u = sizeUV.U;

            while (remaining > 0)
            {
                var newU = u + estimatedSize.U;
                if (newU > viewport.viewportUVEnd.U)
                {
                    sizeUV.V += estimatedSize.V;
                    newU = viewport.viewportUVStart.U + estimatedSize.U;
                }

                u = newU;
                sizeUV.U = Math.Max(sizeUV.U, u);

                remaining--;
            }

            sizeUV.V += estimatedSize.V;
        }

        return new(sizeUV.Width, sizeUV.Height);
    }

    private Rect EstimateViewport()
    {
        var c = this.GetVisualParent();
        var viewport = new Rect();

        if (c is null)
        {
            return viewport;
        }

        while (c is not null)
        {
            if ((c.Bounds.Width != 0 || c.Bounds.Height != 0) &&
                c.TransformToVisual(this) is { } transform)
            {
                viewport = new Rect(0, 0, c.Bounds.Width, c.Bounds.Height)
                    .TransformToAABB(transform);
                break;
            }

            c = c?.GetVisualParent();
        }


        return viewport;
    }

    private void RealizeElements(
        IReadOnlyList<object?> items,
        Size availableSize,
        ref MeasureViewport viewport)
    {
        Debug.Assert(_measureElements is not null);
        Debug.Assert(_realizedElements is not null);
        Debug.Assert(items.Count > 0);

        var index = viewport.anchorIndex;
        var horizontal = Orientation == Orientation.Horizontal;
        var uv = viewport.anchorUV;
        var v = uv.V;
        double maxSizeV = 0;
        var size = new UVSize(Orientation);
        bool firstChildMeasured = false;

        double itemWidth = ItemWidth;
        double itemHeight = ItemHeight;
        bool isItemWidthSet = !double.IsNaN(itemWidth);
        bool isItemHeightSet = !double.IsNaN(itemHeight);

        var childConstraint = new Size(
            isItemWidthSet ? itemWidth : availableSize.Width,
            isItemHeightSet ? itemHeight : availableSize.Height);
        // If the anchor element is at the beginning of, or before, the start of the viewport
        // then we can recycle all elements before it.
        if (uv.V <= viewport.anchorUV.V)
            _realizedElements.RecycleElementsBefore(viewport.anchorIndex, _recycleElement, Orientation);

        // Start at the anchor element and move forwards, realizing elements.
        do
        {
            // Predict if we will place this item in the next row, and if it's not visible, stop realizing it
            if (uv.U + size.U > viewport.viewportUVEnd.U && uv.V + maxSizeV > viewport.viewportUVEnd.V)
            {
                break;
            }

            if (firstChildMeasured)
                childConstraint = new Size(size.Width, size.Height);

            var e = GetOrCreateElement(items, index);
            e.Measure(childConstraint);

            if (!firstChildMeasured)
            {
                size = new UVSize(Orientation,
                    isItemWidthSet ? itemWidth : e.DesiredSize.Width,
                    isItemHeightSet ? itemHeight : e.DesiredSize.Height);

                firstChildMeasured = true;
            }

            maxSizeV = Math.Max(maxSizeV, size.V);

            // Check if the item will exceed the viewport's bounds, and move to next row if it does
            var uEnd = new UVSize(Orientation)
            {
                U = uv.U + size.U,
                V = Math.Max(v, uv.V)
            };

            if (uEnd.U > viewport.viewportUVEnd.U)
            {
                uv.U = viewport.viewportUVStart.U;
                v += maxSizeV;
                maxSizeV = 0;

                uv.V = v;
            }

            _measureElements!.Add(index, e, uv, size);

            uv = new UVSize(Orientation)
            {
                U = uv.U + size.U,
                V = Math.Max(v, uv.V)
            };

            ++index;
        } while (uv.V < viewport.viewportUVEnd.V && index < items.Count);

        // Store the last index and end U position for the desired size calculation.
        viewport.lastIndex = index - 1;
        viewport.realizedEndUV = uv;

        // We can now recycle elements after the last element.
        _realizedElements.RecycleElementsAfter(viewport.lastIndex, _recycleElement, Orientation);

        // Next move backwards from the anchor element, realizing elements.
        index = viewport.anchorIndex - 1;
        uv = viewport.anchorUV;

        while (index >= 0)
        {
            // Predict if this item will be visible, and if not, stop realizing it
            if (uv.U - size.U < viewport.viewportUVStart.U && uv.V <= viewport.viewportUVStart.V)
            {
                break;
            }

            if (firstChildMeasured)
                childConstraint = new Size(size.Width, size.Height);

            var e = GetOrCreateElement(items, index);
            e.Measure(childConstraint);


            if (!firstChildMeasured)
            {
                size = new UVSize(Orientation,
                    isItemWidthSet ? itemWidth : e.DesiredSize.Width,
                    isItemHeightSet ? itemHeight : e.DesiredSize.Height);

                firstChildMeasured = true;
            }

            uv.U -= size.U;

            // Test if the item will be moved to the previous row
            if (uv.U < viewport.viewportUVStart.U)
            {
                var uLength = viewport.viewportUVEnd.U - viewport.viewportUVStart.U;
                var uConstraint = (int)(uLength / size.U) * size.U;
                uv.U = uConstraint - size.U;
                uv.V -= size.V;
            }

            _measureElements!.Add(index, e, uv, size);
            --index;
        }

        // We can now recycle elements before the first element.
        _realizedElements.RecycleElementsBefore(index + 1, _recycleElement, Orientation);
    }

    private Control GetOrCreateElement(IReadOnlyList<object?> items, int index)
    {
        Debug.Assert(ItemContainerGenerator is not null);

        if ((GetRealizedElement(index) ??
             GetRealizedElement(index, ref _focusedIndex, ref _focusedElement) ??
             GetRealizedElement(index, ref _scrollToIndex, ref _scrollToElement)) is { } realized)
            return realized;

        var item = items[index];
        var generator = ItemContainerGenerator!;

        if (generator.NeedsContainer(item, index, out var recycleKey))
        {
            return GetRecycledElement(item, index, recycleKey) ??
                   CreateElement(item, index, recycleKey);
        }
        else
        {
            return GetItemAsOwnContainer(item, index);
        }
    }

    private Control GetItemAsOwnContainer(object? item, int index)
    {
        Debug.Assert(ItemContainerGenerator is not null);

        var controlItem = (Control)item!;
        var generator = ItemContainerGenerator!;

        if (!controlItem.IsSet(RecycleKeyProperty))
        {
            generator.PrepareItemContainer(controlItem, controlItem, index);
            AddInternalChild(controlItem);
            controlItem.SetValue(RecycleKeyProperty, s_itemIsItsOwnContainer);
            generator.ItemContainerPrepared(controlItem, item, index);
        }

        controlItem.IsVisible = true;
        return controlItem;
    }

    private Control? GetRealizedElement(int index)
    {
        return _realizedElements?.GetElement(index);
    }

    private static Control? GetRealizedElement(
        int index,
        ref int specialIndex,
        ref Control? specialElement)
    {
        if (specialIndex == index)
        {
            Debug.Assert(specialElement is not null);

            var result = specialElement;
            specialIndex = -1;
            specialElement = null;
            return result;
        }

        return null;
    }

    private Control? GetRecycledElement(object? item, int index, object? recycleKey)
    {
        Debug.Assert(ItemContainerGenerator is not null);

        if (recycleKey is null)
            return null;

        var generator = ItemContainerGenerator!;

        if (_recyclePool?.TryGetValue(recycleKey, out var recyclePool) == true && recyclePool.Count > 0)
        {
            var recycled = recyclePool.Pop();
            recycled.IsVisible = true;
            generator.PrepareItemContainer(recycled, item, index);
            generator.ItemContainerPrepared(recycled, item, index);
            return recycled;
        }

        return null;
    }

    private Control CreateElement(object? item, int index, object? recycleKey)
    {
        Debug.Assert(ItemContainerGenerator is not null);

        var generator = ItemContainerGenerator!;
        var container = generator.CreateContainer(item, index, recycleKey);

        container.SetValue(RecycleKeyProperty, recycleKey);
        generator.PrepareItemContainer(container, item, index);
        AddInternalChild(container);
        generator.ItemContainerPrepared(container, item, index);

        return container;
    }

    private void RecycleElement(Control element, int index)
    {
        Debug.Assert(ItemContainerGenerator is not null);

        _scrollViewer?.UnregisterAnchorCandidate(element);

        var recycleKey = element.GetValue(RecycleKeyProperty);

        if (recycleKey is null)
        {
            RemoveInternalChild(element);
        }
        else if (recycleKey == s_itemIsItsOwnContainer)
        {
            element.IsVisible = false;
        }
        else if (element.IsKeyboardFocusWithin)
        {
            _focusedElement = element;
            _focusedIndex = index;
            _focusedElement.LostFocus += OnUnrealizedFocusedElementLostFocus;
        }
        else
        {
            ItemContainerGenerator!.ClearItemContainer(element);
            PushToRecyclePool(recycleKey, element);
            element.IsVisible = false;
        }
    }

    private void RecycleElementOnItemRemoved(Control element)
    {
        Debug.Assert(ItemContainerGenerator is not null);

        var recycleKey = element.GetValue(RecycleKeyProperty);

        if (recycleKey is null || recycleKey == s_itemIsItsOwnContainer)
        {
            RemoveInternalChild(element);
        }
        else
        {
            ItemContainerGenerator!.ClearItemContainer(element);
            PushToRecyclePool(recycleKey, element);
            element.IsVisible = false;
        }
    }

    private void PushToRecyclePool(object recycleKey, Control element)
    {
        _recyclePool ??= new();

        if (!_recyclePool.TryGetValue(recycleKey, out var pool))
        {
            pool = new();
            _recyclePool.Add(recycleKey, pool);
        }

        pool.Push(element);
    }

    private void UpdateElementIndex(Control element, int oldIndex, int newIndex)
    {
        Debug.Assert(ItemContainerGenerator is not null);

        ItemContainerGenerator.ItemContainerIndexChanged(element, oldIndex, newIndex);
    }

    private void OnEffectiveViewportChanged(object? sender, EffectiveViewportChangedEventArgs e)
    {
        var horizontal = Orientation == Orientation.Horizontal;
        var oldViewportStartU = horizontal ? _viewport.Left : _viewport.Top;
        var oldViewportEndU = horizontal ? _viewport.Right : _viewport.Bottom;
        var oldViewportStartV = horizontal ? _viewport.Top : _viewport.Left;
        var oldViewportEndV = horizontal ? _viewport.Bottom : _viewport.Right;

        _viewport = e.EffectiveViewport.Intersect(new(Bounds.Size));
        _isWaitingForViewportUpdate = false;

        var newViewportStartU = horizontal ? _viewport.Left : _viewport.Top;
        var newViewportEndU = horizontal ? _viewport.Right : _viewport.Bottom;
        var newViewportStartV = horizontal ? _viewport.Top : _viewport.Left;
        var newViewportEndV = horizontal ? _viewport.Bottom : _viewport.Right;

        if (!MathUtilities.AreClose(oldViewportStartU, newViewportStartU) ||
            !MathUtilities.AreClose(oldViewportEndU, newViewportEndU) ||
            !MathUtilities.AreClose(oldViewportStartV, newViewportStartV) ||
            !MathUtilities.AreClose(oldViewportEndV, newViewportEndV))
        {
            InvalidateMeasure();
        }
    }

    private void OnUnrealizedFocusedElementLostFocus(object? sender, RoutedEventArgs e)
    {
        if (_unrealizedFocusedElement is null || sender != _unrealizedFocusedElement)
            return;

        _unrealizedFocusedElement.LostFocus -= OnUnrealizedFocusedElementLostFocus;
        RecycleElement(_unrealizedFocusedElement, _unrealizedFocusedIndex);
        _unrealizedFocusedElement = null;
        _unrealizedFocusedIndex = -1;
    }


    private struct MeasureViewport
    {
        public int anchorIndex;
        public UVSize anchorUV;
        public UVSize viewportUVStart;
        public UVSize viewportUVEnd;
        public UVSize realizedEndUV;
        public int lastIndex;
        public bool viewportIsDisjunct;
    }
}