namespace SimpleShellSample;

public partial class SchoolPage : ContentPage
{
	public SchoolPage()
	{
		InitializeComponent();
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("MainPage");
	}
}