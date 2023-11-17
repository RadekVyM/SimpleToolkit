namespace Sample.SimpleShellTopBar.Views.Pages;

public partial class FirstRootPage : ContentPage
{
	public FirstRootPage()
	{
		InitializeComponent();
	}

    private async void ButtonClicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync(nameof(FirstDetailPage));
    }
}