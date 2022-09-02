namespace SimpleToolkit.Core
{
    public interface IContentButton : IContentView
    {
        void OnClicked();
        void OnPressed(Point pressPosition);
        void OnReleased(Point releasePosition);
    }
}
