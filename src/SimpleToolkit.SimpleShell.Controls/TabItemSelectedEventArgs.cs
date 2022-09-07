namespace SimpleToolkit.SimpleShell.Controls
{
    public delegate void TabItemSelectedEventHandler(object sender, TabItemSelectedEventArgs e);

    /// <summary>
    /// Arguments of a tab selection event.
    /// </summary>
    public class TabItemSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// The selected <see cref="BaseShellItem"/> item.
        /// </summary>
        public BaseShellItem ShellItem { get; internal set; }
    }
}
