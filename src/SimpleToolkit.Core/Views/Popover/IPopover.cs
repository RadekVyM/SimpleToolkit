namespace SimpleToolkit.Core
{
    public interface IPopover : IElement
    {
        View Content { get; set; }

        void Show(View parentView);
        void Hide();
    }
}
