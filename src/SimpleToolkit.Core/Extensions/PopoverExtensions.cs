namespace SimpleToolkit.Core;

/// <summary>
/// Extension methods for Popover control.
/// </summary>
public static class PopoverExtensions
{
    /// <summary>
    /// Shows a popover that is attached to the view.
    /// </summary>
    /// <param name="parentView">The view to which the popover is attached.</param>
    public static void ShowAttachedPopover(this View parentView)
    {
        var popover = Popover.GetAttachedPopover(parentView);
        popover.Show(parentView);
    }

    /// <summary>
    /// Hides a popover that is attached to the view.
    /// </summary>
    /// <param name="parentView">The view to which the popover is attached.</param>
    public static void HideAttachedPopover(this View parentView)
    {
        var popover = Popover.GetAttachedPopover(parentView);
        popover.Hide();
    }
}