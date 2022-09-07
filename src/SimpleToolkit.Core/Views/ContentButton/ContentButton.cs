namespace SimpleToolkit.Core
{
    /// <summary>
    /// Button that can hold whatever content you want.
    /// </summary>
    [ContentProperty(nameof(Content))]
    public class ContentButton : ContentView, IContentButton
    {
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

        public virtual void OnClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnPressed(Point pressPosition)
        {
            VisualStateManager.GoToState(this, "Pressed");
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
