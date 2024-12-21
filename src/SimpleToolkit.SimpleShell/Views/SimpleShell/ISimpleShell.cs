namespace SimpleToolkit.SimpleShell;

/// <summary>
/// Shell that lets you define your custom navigation experience. 
/// </summary>
public interface ISimpleShell : IView, IElement, ITransform, IShellController, IPageController, IVisualElementController, IElementController, IPageContainer<Page>
{
    /// <summary>
    /// Gets or sets the content of this shell.
    /// </summary>
    IView Content { get; set; }
    /// <summary>
    /// View that wraps root pages.
    /// </summary>
    IView? RootPageContainer { get; set; }
    /// <summary>
    /// The currently selected <see cref="Page"/>.
    /// </summary>
    new Page CurrentPage { get; }
    /// <summary>
    /// The currently selected <see cref="ShellItem"/>.
    /// </summary>
    ShellItem CurrentItem { get; set; }
    /// <summary>
    /// The currently selected <see cref="ShellContent"/>.
    /// </summary>
    ShellContent CurrentShellContent { get; }
    /// <summary>
    /// The currently selected <see cref="ShellSection"/>.
    /// </summary>
    ShellSection CurrentShellSection { get; }
    /// <summary>
    /// All <see cref="ShellSection"/> items of this shell.
    /// </summary>
    IReadOnlyList<ShellSection> ShellSections { get; }
    /// <summary>
    /// All <see cref="ShellContent"/> items of this shell.
    /// </summary>
    IReadOnlyList<ShellContent> ShellContents { get; }
    /// <summary>
    /// All <see cref="FlyoutItem"/> items of this shell.
    /// </summary>
    IReadOnlyList<FlyoutItem> FlyoutItems { get; }
    /// <summary>
    /// All <see cref="TabBar"/> items of this shell.
    /// </summary>
    IReadOnlyList<TabBar> TabBars { get; }
}