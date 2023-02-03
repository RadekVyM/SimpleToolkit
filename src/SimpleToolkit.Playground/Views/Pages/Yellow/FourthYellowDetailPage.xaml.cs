namespace SimpleToolkit.SimpleShell.Playground.Views.Pages;

public partial class FourthYellowDetailPage : ContentPage
{
	public FourthYellowDetailPage()
	{
		InitializeComponent();
	}

    private async void TwiceClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"../..", true);
    }

    private async void GreenClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(GreenPage)}", true);
    }

    private async void GreenDetailClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(GreenPage)}/{nameof(FirstGreenDetailPage)}", true);
    }
}
