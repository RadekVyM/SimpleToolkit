using System.Windows.Input;

namespace SimpleToolkit.SimpleButton;

/// <summary>
/// Button that can hold whatever content you want.
/// </summary>
[ContentProperty(nameof(Content))]
public class SimpleButton : Border, ISimpleButton
{
    private const string PressedState = "Pressed";

    private bool isPressed = false;

    /// <summary>
    /// Event that fires when the button is clicked.
    /// </summary>
    public event EventHandler? Clicked;
    /// <summary>
    /// Event that fires when the button is pressed.
    /// </summary>
    public event EventHandler<SimpleButtonEventArgs>? Pressed;
    /// <summary>
    /// Event that fires when the button is released.
    /// </summary>
    public event EventHandler<SimpleButtonEventArgs>? Released;

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SimpleButton), defaultValue: null, propertyChanging: OnCommandChanging, propertyChanged: OnCommandChanged);

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(SimpleButton), defaultValue: null,
        propertyChanged: (bindable, oldvalue, newvalue) => CommandCanExecuteChanged(bindable, EventArgs.Empty));

    public virtual ICommand? Command
    {
        get => GetValue(CommandProperty) as ICommand;
        set => SetValue(CommandProperty, value);
    }

    public virtual object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }


    public virtual void OnClicked()
    {
        if (!IsEnabled)
            return;

        Clicked?.Invoke(this, EventArgs.Empty);

        if (Command?.CanExecute(CommandParameter) == true)
            Command?.Execute(CommandParameter);
    }

    public virtual void OnPressed(Point pressPosition)
    {
        if (!IsEnabled)
            return;

        UpdatePressed(true);

        Pressed?.Invoke(this, new SimpleButtonEventArgs
        {
            InteractionPosition = pressPosition
        });
    }

    public virtual void OnReleased(Point releasePosition)
    {
        if (!IsEnabled && !isPressed)
            return;

        UpdatePressed(false);

        Released?.Invoke(this, new SimpleButtonEventArgs
        {
            InteractionPosition = releasePosition
        });
    }

    private void UpdatePressed(bool pressed)
    {
        isPressed = pressed;
        UpdateState();
    }

    private void UpdateState()
    {
        if (isPressed)
            VisualStateManager.GoToState(this, PressedState);
        else
            GoToDefaultState();
    }

    private void GoToDefaultState()
    {
        if (!IsEnabled)
            VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Disabled);
        else
            VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Normal);

        if (IsEnabled && IsFocused)
        {
            // Focus needs to be handled independently; otherwise, if no actual Focus state is supplied
            // in the control's visual states, the state can end up stuck in PointerOver after the pointer
            // exits and the control still has focus.
            VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Focused);
        }
    }

    private void OnCommandCanExecuteChanged(object? sender, EventArgs e)
        => CommandCanExecuteChanged(this, e);

    private static void CommandChanged(SimpleButton button)
    {
        if (button.Command is not null)
            CommandCanExecuteChanged(button, EventArgs.Empty);
        else
            button.SetValueFromRenderer(IsEnabledProperty, true);
    }

    private static void CommandCanExecuteChanged(object? sender, EventArgs e)
    {
        if (sender is SimpleButton button && button.Command is not null)
        {
            button.SetValueFromRenderer(IsEnabledProperty, button.Command.CanExecute(button.CommandParameter));
            button.UpdateState();
        }
    }

    private static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SimpleButton button)
            return;

        if (newValue is ICommand newCommand)
            newCommand.CanExecuteChanged += button.OnCommandCanExecuteChanged;

        CommandChanged(button);
    }

    private static void OnCommandChanging(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SimpleButton button && oldValue is ICommand oldCommand)
            oldCommand.CanExecuteChanged -= button.OnCommandCanExecuteChanged;
    }
}