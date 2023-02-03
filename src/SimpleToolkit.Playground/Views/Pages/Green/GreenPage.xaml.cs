namespace SimpleToolkit.SimpleShell.Playground.Views.Pages;

public partial class GreenPage : ContentPage
{
	public GreenPage()
	{
		InitializeComponent();
    }

    private async void FirstDetailClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FirstGreenDetailPage));
    }

    private async void YellowDetailClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(YellowPage)}/{nameof(FirstYellowDetailPage)}/{nameof(SecondYellowDetailPage)}");
    }
}