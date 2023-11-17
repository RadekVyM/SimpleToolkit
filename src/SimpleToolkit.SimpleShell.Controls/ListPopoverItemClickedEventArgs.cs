namespace SimpleToolkit.SimpleShell.Controls;

public delegate void ListPopoverItemSelectedEventHandler(object sender, ListPopoverItemSelectedEventArgs e);

/// <summary>
/// Arguments of <see cref="ListPopover"/> selection event.
/// </summary>
public class ListPopoverItemSelectedEventArgs : EventArgs
{
    /// <summary>
    /// The selected item.
    /// </summary>
    public object Item { get; internal set; }
}