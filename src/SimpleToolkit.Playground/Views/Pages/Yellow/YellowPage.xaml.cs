using SimpleToolkit.Core;

namespace SimpleToolkit.SimpleShell.Playground.Views.Pages;

public partial class YellowPage : ContentPage
{
	public YellowPage()
	{
		InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        System.Diagnostics.Debug.WriteLine($"{nameof(YellowPage)}: Back button pressed");
        return base.OnBackButtonPressed();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FirstYellowDetailPage), true);
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/{nameof(FirstYellowDetailPage)}/{nameof(SecondYellowDetailPage)}", true);
    }
}