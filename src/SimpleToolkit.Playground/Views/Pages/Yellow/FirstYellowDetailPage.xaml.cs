namespace SimpleToolkit.SimpleShell.Playground.Views.Pages;

public partial class FirstYellowDetailPage : ContentPage
{
	public FirstYellowDetailPage()
	{
		InitializeComponent();
	}

    protected override bool OnBackButtonPressed()
    {
        System.Diagnostics.Debug.WriteLine($"{nameof(FirstYellowDetailPage)}: Back button pressed");
        return base.OnBackButtonPressed();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SecondYellowDetailPage), true);
    }
}