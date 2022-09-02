namespace SimpleToolkit.Core
{
    [ContentProperty(nameof(Content))]
    public class ContentButton : ContentView, IContentButton
    {
        public event EventHandler Clicked;
        public event EventHandler<ContentButtonEventArgs> Pressed;
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
