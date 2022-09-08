namespace SimpleToolkit.Core
{
    /// <summary>
    /// Popover that can be anchored to a view.
    /// </summary>
    public interface IPopover : IElement
    {
        /// <summary>
        /// Content of the popover
        /// </summary>
        View Content { get; set; }
        /// <summary>
        /// Shows the popover anchored to the view.
        /// </summary>
        /// <param name="parentView">The view to which the popover is anchored.</param>
        void Show(View parentView);
        /// <summary>
        /// Hides the popover.
        /// </summary>
        void Hide();
    }
}
