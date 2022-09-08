namespace SimpleToolkit.SimpleShell
{
    /// <summary>
    /// A shell that lets you define your custom navigation experience. 
    /// </summary>
    public interface ISimpleShell : IFlyoutView, IView, IElement, ITransform, IShellController, IPageController, IVisualElementController, IElementController, IPageContainer<Page>
    {
        /// <summary>
        /// Gets or sets the content of this shell.
        /// </summary>
        IView Content { get; set; }
        /// <summary>
        /// The currently selected <see cref="Page"/>.
        /// </summary>
        new Page CurrentPage { get; }
        /// <summary>
        /// The currently selected <see cref="ShellItem"/> or <see cref="FlyoutItem"/>.
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
    }
}
