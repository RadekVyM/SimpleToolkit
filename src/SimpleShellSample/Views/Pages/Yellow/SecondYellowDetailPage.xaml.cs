namespace SimpleShellSample.Views.Pages;

public partial class SecondYellowDetailPage : ContentPage
{
	public SecondYellowDetailPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ThirdYellowDetailPage));
    }
}