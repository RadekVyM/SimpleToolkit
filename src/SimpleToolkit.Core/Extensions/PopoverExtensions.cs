namespace SimpleToolkit.Core
{
    public static class PopoverExtensions
    {
        public static void ShowAttachedPopover(this View parentView)
        {
            var popover = Popover.GetAttachedPopover(parentView);
            popover.Show(parentView);
        }

        public static void HideAttachedPopover(this View parentView)
        {
            var popover = Popover.GetAttachedPopover(parentView);
            popover.Hide();
        }
    }
}
