using SimpleToolkit.Core;

namespace SimpleToolkit.SimpleShell.Playground.Views.Pages;

public partial class PurplePage : ContentPage
{
	public PurplePage()
	{
		InitializeComponent();
	}

	private void ContentButton_Clicked(object sender, EventArgs e)
	{
		var button = sender as ContentButton;

		var from = button.TranslationX;
		var to = button.TranslationX is 0 ? button.Width / 2 : 0;

		var animation = new Animation(d =>
		{
			button.TranslationX = d;
		}, from, to);

		animation.Commit(button, "Animation");
    }
}