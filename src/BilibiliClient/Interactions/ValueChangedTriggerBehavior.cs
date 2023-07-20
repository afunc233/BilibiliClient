using System.Linq;
using Avalonia;
using Avalonia.Reactive;
using Avalonia.Xaml.Interactivity;

namespace BilibiliClient.Interactions;


public class ValueChangedTriggerBehavior : Trigger
{
    static ValueChangedTriggerBehavior()
    {
        BindingProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<object?>>(OnValueChanged));
    }

    /// <summary>
    /// Identifies the <seealso cref="Binding"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<object?> BindingProperty =
        AvaloniaProperty.Register<ValueChangedTriggerBehavior, object?>(nameof(Binding));

    /// <summary>
    /// Gets or sets the bound object that the <see cref="ValueChangedTriggerBehavior"/> will listen to. This is a avalonia property.
    /// </summary>
    public object? Binding
    {
        get => GetValue(BindingProperty);
        set => SetValue(BindingProperty, value);
    }

    private static void OnValueChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not ValueChangedTriggerBehavior behavior || behavior.AssociatedObject is null)
        {
            return;
        }

        var binding = behavior.Binding;
        if (binding is null) return;
        var result = Interaction.ExecuteActions(behavior.AssociatedObject, behavior.Actions, args);
        if (result.Any(it => it is bool and false))
        {

        }
    }
}