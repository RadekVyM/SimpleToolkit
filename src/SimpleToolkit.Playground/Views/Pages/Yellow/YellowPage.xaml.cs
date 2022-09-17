using SimpleToolkit.Core;

namespace SimpleToolkit.SimpleShell.Playground.Views.Pages;

public partial class YellowPage : ContentPage
{
	public YellowPage()
	{
		InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

#if ANDROID
        this.Window.SetStatusBarAppearance(Colors.Crimson, true);
        this.Window.SetNavigationBarAppearance(Colors.NavajoWhite, false);
#endif
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
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
        var button = sender as View;

        button.ShowAttachedPopover();
    }
}