namespace SimpleShellSample.Views.Pages;

public partial class GreenPage : ContentPage
{
	public GreenPage()
	{
		InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FirstGreenDetailPage));
    }
}