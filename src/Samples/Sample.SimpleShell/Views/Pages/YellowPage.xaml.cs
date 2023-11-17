namespace Sample.SimpleShell.Views.Pages;

public partial class YellowPage : ContentPage
{
	public YellowPage()
	{
		InitializeComponent();
    }

    private async void DetailPageButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(YellowDetailPage));
    }
}