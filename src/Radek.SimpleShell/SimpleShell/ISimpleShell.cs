namespace Radek.SimpleShell
{
    public interface ISimpleShell : IFlyoutView, IView, IElement, ITransform, IShellController, IPageController, IVisualElementController, IElementController, IPageContainer<Page>
    {
        IView Content { get; set; }
        ShellItem CurrentItem { get; set; }
    }
}
