namespace SimpleToolkit.SimpleShell.Controls
{
    /// <summary>
    /// Tab bar that is styled according to the selected <see cref="DesignLanguage"/>.
    /// </summary>
    public interface ITabBar
    {
        IEnumerable<BaseShellItem> Items { get; set; }
        IReadOnlyList<BaseShellItem> HiddenItems { get; }
        BaseShellItem SelectedItem { get; set; }
        DesignLanguage DesignLanguage { get; set; }
        Color TextColor { get; set; }
        Color TextSelectionColor { get; set; }
        Color IconColor { get; set; }
        Color IconSelectionColor { get; set; }
        Brush PrimaryBrush { get; set; }
        string MoreTitle { get; set; }
        ImageSource MoreIcon { get; set; }
        double ItemWidthRequest { get; set; }
        bool IsScrollable { get; set; }
        LayoutAlignment ItemsAlignment { get; set; }
        bool ShowButtonWhenMoreItemsDoNotFit { get; set; }
        bool ShowMenuOnMoreButtonClick { get; set; }

        /// <summary>
        /// Event that fires when an item is selected.
        /// </summary>
        event TabItemSelectedEventHandler ItemSelected;

        /// <summary>
        /// Event that fires when the "More" button is clicked.
        /// </summary>
        event EventHandler MoreButtonClicked;
    }
}