namespace SimpleToolkit.Core;

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
    /// Alignment of the popover to its anchor.
    /// This property is only for Android and Windows.
    /// </summary>
    PopoverAlignment Alignment { get; set; }
    /// <summary>
    /// Permitted arrow directions of an iOS popover.
    /// This property is only for iOS.
    /// </summary>
    PopoverArrowDirection PermittedArrowDirections { get; set; }
    /// <summary>
    /// Whether the default platform-specific styling of the popover should be used.
    /// </summary>
    bool UseDefaultStyling { get; set; }
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