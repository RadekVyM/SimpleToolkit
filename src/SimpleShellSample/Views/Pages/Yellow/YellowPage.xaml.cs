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
}