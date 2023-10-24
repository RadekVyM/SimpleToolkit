using SimpleToolkit.Core;

namespace SimpleToolkit.SimpleShell.Controls;

/// <summary>
/// Popover containing a list of items that is styled according to the selected <see cref="DesignLanguage"/>.
/// </summary>
public interface IListPopover : IPopover
{
    IEnumerable<object> Items { get; set; }
    DesignLanguage DesignLanguage { get; set; }
    Brush Background { get; set; }
    Color IconColor { get; set; }
    Color IconSelectionColor { get; set; }
    double MaximumWidthRequest { get; set; }
    double MinimumWidthRequest { get; set; }
    object SelectedItem { get; set; }
    Brush SelectionBrush { get; set; }
    Color TextColor { get; set; }
    Color TextSelectionColor { get; set; }
    
    /// <summary>
    /// Event that fires when an item is selected.
    /// </summary>
    event ListPopoverItemSelectedEventHandler ItemSelected;
}