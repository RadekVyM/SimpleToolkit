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

    protected override void OnAppearing()
    {
        System.Diagnostics.Debug.WriteLine("OnAppearing");
        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        System.Diagnostics.Debug.WriteLine("OnDisappearing");
        base.OnDisappearing();
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        System.Diagnostics.Debug.WriteLine("OnNavigatingFrom");
        base.OnNavigatingFrom(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        System.Diagnostics.Debug.WriteLine("OnNavigatedFrom");
        base.OnNavigatedFrom(args);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        System.Diagnostics.Debug.WriteLine("OnNavigatedTo");
        base.OnNavigatedTo(args);
    }
}