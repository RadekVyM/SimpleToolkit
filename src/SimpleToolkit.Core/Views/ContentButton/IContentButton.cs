using System.Windows.Input;

namespace SimpleToolkit.Core
{
    /// <summary>
    /// Button that can hold whatever content you want.
    /// </summary>
    public interface IContentButton : IContentView
    {
        /// <summary>
        /// Gets or sets the command to invoke when the button is clicked. This is a bindable property.
        /// </summary>
        ICommand Command { get; set; }
        /// <summary>
        /// Gets or sets the parameter to pass to the <see cref="Command"/> property. This is a bindable property.
        /// </summary>
        object CommandParameter { get; set; }

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
