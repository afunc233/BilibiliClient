using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Mvvm.Input;

namespace BilibiliClient.Interactions;

public class Scroll2EndTriggerBehavior : Trigger
{
    /// <summary>
    /// Identifies the <seealso cref="Distance2End"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<double> BindingProperty =
        AvaloniaProperty.Register<Scroll2EndTriggerBehavior, double>(nameof(Distance2End), 200d);

    /// <summary>
    /// Gets or sets the bound object that the <see cref="Scroll2EndTriggerBehavior"/> will listen to. This is a avalonia property.
    /// </summary>
    public double Distance2End
    {
        get => GetValue(BindingProperty);
        set => SetValue(BindingProperty, value);
    }

    /// <summary>
    /// Identifies the <seealso cref="LoadMoreDataCmd"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<ICommand> LoadMoreDataCmdProperty =
        AvaloniaProperty.Register<ValueChangedTriggerBehavior, ICommand>(nameof(LoadMoreDataCmd));

    /// <summary>
    /// Gets or sets the bound object that the <see cref="ValueChangedTriggerBehavior"/> will listen to. This is a avalonia property.
    /// </summary>
    public ICommand LoadMoreDataCmd
    {
        get => GetValue(LoadMoreDataCmdProperty);
        set => SetValue(LoadMoreDataCmdProperty, value);
    }


    protected override void OnAttachedToVisualTree()
    {
        base.OnAttachedToVisualTree();
        if (AssociatedObject is not ItemsControl control) return;
        control.AddHandler(ScrollViewer.ScrollChangedEvent, ScrollViewerOnScrollChanged);
    }

    protected override void OnDetachedFromVisualTree()
    {
        base.OnDetachedFromVisualTree();
        if (AssociatedObject is not ItemsControl control) return;

        control.RemoveHandler(ScrollViewer.ScrollChangedEvent, ScrollViewerOnScrollChanged);
    }

    private async void ScrollViewerOnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (AssociatedObject is not ItemsControl control) return;

        var scrollViewer = control.GetTemplateChildren()?.OfType<ScrollViewer>().FirstOrDefault();
        if (scrollViewer is null)
        {
            return;
        }

        var extent = scrollViewer.Extent;
        var offset = scrollViewer.Offset;
        if (extent.Height - scrollViewer.Viewport.Height < 0.5d)
        {
            // Layout 那一次，调用 Command
            await CallCommand(scrollViewer, e);
        }
        else if (extent.Height - (offset.Y + scrollViewer.Viewport.Height) < Distance2End)
        {
            await CallCommand(scrollViewer, e);
        }
    }

    private async Task CallCommand(ScrollViewer scrollViewer, object e)
    {
        while (true)
        {
            if (LoadMoreDataCmd is AsyncRelayCommand asyncRelayCommand)
            {
                if (asyncRelayCommand.CanExecute(e))
                {
                    await asyncRelayCommand.ExecuteAsync(e);
                    var extent = scrollViewer.Extent;
                    if (extent.Height - scrollViewer.Viewport.Height < 0.5d)
                    {
                        continue;
                    }
                }
            }
            else
            {
                if (LoadMoreDataCmd?.CanExecute(e) ?? false)
                {
                    LoadMoreDataCmd?.Execute(e);
                }
            }

            var result = Interaction.ExecuteActions(AssociatedObject, Actions, e);
            if (result.Any(it => it is bool and false))
            {
            }

            break;
        }
    }
}