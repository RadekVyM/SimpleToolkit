namespace SimpleToolkit.SimpleShell.Controls
{
    public delegate void ListPopoverItemSelectedEventHandler(object sender, ListPopoverItemSelectedEventArgs e);

    public class ListPopoverItemSelectedEventArgs : EventArgs
    {
        public object Item { get; internal set; }
    }
}