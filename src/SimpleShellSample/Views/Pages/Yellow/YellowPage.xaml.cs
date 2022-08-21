using Radek.SimpleShell.Controls;

namespace SimpleShellSample.Views.Pages;

public partial class YellowPage : ContentPage
{
	public YellowPage()
	{
		InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FirstYellowDetailPage));
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/{nameof(FirstYellowDetailPage)}/{nameof(SecondYellowDetailPage)}");
    }

    private void Button_Clicked_2(object sender, EventArgs e)
    {
        var button = sender as Button;

        button.ShowAttachedPopover();
    }
}