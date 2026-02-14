namespace Playground.Original.Views.Pages;

public partial class ImageDetailPage : ContentPage, IQueryAttributable
{
    public ImageDetailPage()
	{
		InitializeComponent();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
		var imageSource = query["ImageSource"] as ImageSource;
        image.Source = imageSource;
    }
}