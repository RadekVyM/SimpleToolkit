using SimpleToolkit.Core;

namespace Playground.Original.Views.Pages;

public partial class PurplePage : ContentPage
{
	public PurplePage()
	{
		InitializeComponent();
	}

    public void ContentButton_Clicked(object sender, EventArgs e)
    {
		var ve = sender as View;
		var popover = Popover.GetAttachedPopover(ve);
		var label = popover.Content as Label;

        var bounds = ve.Bounds;
		var frame = ve.Frame;
		var myBounds = ve.GetBounds();
		var myRelativeBounds = ve.GetBounds(relativeTo: this.Content);

		label.Text = $"{RectToShortString(bounds)}\n{RectToShortString(frame)}\n{RectToShortString(myBounds)}\n{RectToShortString(myRelativeBounds)}";

        ve.ShowAttachedPopover();
    }

	private string RectToShortString(Rect rect) =>
		$"X={Math.Round(rect.X, 2)} Y={Math.Round(rect.Y, 2)} Width={Math.Round(rect.Width, 2)} Height={Math.Round(rect.Height, 2)}";

    private void Button_Clicked(object sender, EventArgs e)
    {
		translateContentView.AbortAnimation("Animation");

        var to = translateContentView.TranslationX == 50 ? -50 : 50;

        var animation = new Animation((v) =>
		{
			translateContentView.TranslationX = to * v;
        });

		animation.Commit(translateContentView, "Animation");
    }
}