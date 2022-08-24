namespace Radek.SimpleShell.Controls
{
    public delegate void ListPopoverItemClickedEventHandler(object sender, ListPopoverItemClickedEventArgs e);

    public class ListPopoverItemClickedEventArgs : EventArgs
    {
        public object Item { get; internal set; }
    }
}