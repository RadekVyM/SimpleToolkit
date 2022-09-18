using System.Windows.Input;

namespace SimpleToolkit.Core
{
    /// <summary>
    /// Button that can hold whatever content you want.
    /// </summary>
    [ContentProperty(nameof(Content))]
    public class ContentButton : ContentView, IContentButton
    {
        private const string PressedState = "Pressed";

        /// <summary>
        /// Event that fires when the button is clicked.
        /// </summary>
        public event EventHandler Clicked;
        /// <summary>
        /// Event that fires when the button is pressed.
        /// </summary>
        public event EventHandler<ContentButtonEventArgs> Pressed;
        /// <summary>
        /// Event that fires when the button is released.
        /// </summary>
        public event EventHandler<ContentButtonEventArgs> Released;

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ContentButton), defaultValue: null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ContentButton), defaultValue: null);

        public virtual ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public virtual object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public virtual void OnClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);

            if (Command?.CanExecute(CommandParameter) == true)
            {
                Command?.Execute(CommandParameter);
            }
        }

        public virtual void OnPressed(Point pressPosition)
        {
            VisualStateManager.GoToState(this, PressedState);
            Pressed?.Invoke(this, new ContentButtonEventArgs
            {
                InteractionPosition = pressPosition
            });
        }

        public virtual void OnReleased(Point releasePosition)
        {
            GoToDefaultState();
            Released?.Invoke(this, new ContentButtonEventArgs
            {
                InteractionPosition = releasePosition
            });
        }

        private void GoToDefaultState()
        {
            if (!IsEnabled)
                VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Disabled);
            else if (IsFocused)
                VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Focused);
            else
                VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Normal);
        }
    }
}
