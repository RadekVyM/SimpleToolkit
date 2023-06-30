using SimpleToolkit.Core;

namespace SimpleToolkit.SimpleShell.Playground.Views.Pages;

public partial class ImagesPage : ContentPage
{
	public ImagesPage()
	{
		InitializeComponent();
	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		var image = sender as Image;
		var bounds = image.GetBounds();

        Shell.Current.GoToAsync(nameof(ImageDetailPage), new Dictionary<string, object>
		{
			["ImageSource"] = image.Source,
			["ImageRect"] = bounds,
			["SetImageOpacity"] = (double opacity) => image.Opacity = opacity,
        });
    }
}
