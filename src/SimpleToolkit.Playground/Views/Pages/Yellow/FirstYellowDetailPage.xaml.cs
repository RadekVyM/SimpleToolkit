namespace SimpleToolkit.SimpleShell.Playground.Views.Pages;

public partial class FirstYellowDetailPage : ContentPage
{
	public FirstYellowDetailPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SecondYellowDetailPage));
    }
}