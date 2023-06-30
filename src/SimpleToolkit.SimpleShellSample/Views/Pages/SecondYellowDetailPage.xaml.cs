namespace SimpleToolkit.SimpleShell.Playground.Views.Pages;

public partial class SecondYellowDetailPage : ContentPage
{
	public SecondYellowDetailPage()
	{
		InitializeComponent();
	}

    private async void RootButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopToRootAsync();
    }

    private async void ThirdButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ThirdYellowDetailPage), true);
    }

    private async void FourthButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FourthYellowDetailPage), false);
    }
}