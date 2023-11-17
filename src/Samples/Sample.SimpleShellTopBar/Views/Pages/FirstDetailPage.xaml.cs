namespace Sample.SimpleShellTopBar.Views.Pages;

public partial class FirstDetailPage : ContentPage
{
	public FirstDetailPage()
	{
		InitializeComponent();
	}

    private async void ButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SecondDetailPage));
    }
}