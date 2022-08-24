namespace Radek.SimpleShell.Controls
{
    public delegate void TabItemSelectedEventHandler(object sender, TabItemSelectedEventArgs e);

    public class TabItemSelectedEventArgs : EventArgs
    {
        public BaseShellItem ShellItem { get; internal set; }
    }
}
