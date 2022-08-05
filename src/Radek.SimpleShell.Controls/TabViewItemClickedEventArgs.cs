namespace Radek.SimpleShell.Controls
{
    public delegate void TabViewItemSelectedEventHandler(object sender, TabViewItemSelectedEventArgs e);

    public class TabViewItemSelectedEventArgs : EventArgs
    {
        public BaseShellItem ShellItem { get; internal set; }
    }
}
