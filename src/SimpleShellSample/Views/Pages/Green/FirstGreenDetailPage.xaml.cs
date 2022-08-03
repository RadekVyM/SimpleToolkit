namespace SimpleShellSample.Views.Pages;

public partial class FirstGreenDetailPage : ContentPage
{
	public FirstGreenDetailPage()
	{
		InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(RedPage)}");
    }
}