namespace SimpleToolkit.Core
{
    /// <summary>
    /// Button that can hold whatever content you want.
    /// </summary>
    public interface IContentButton : IContentView
    {
        /// <summary>
        /// Method that is called when the button is clicked.
        /// </summary>
        void OnClicked();
        /// <summary>
        /// Method that is called when the button is pressed.
        /// </summary>
        void OnPressed(Point pressPosition);
        /// <summary>
        /// Method that is called when the button is released.
        /// </summary>
        void OnReleased(Point releasePosition);
    }
}
