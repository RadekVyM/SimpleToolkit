using SimpleToolkit.Core;

namespace SimpleToolkit.SimpleShell.Playground.Views.Pages;

public partial class GrayPage : ContentPage
{
	public GrayPage()
	{
		InitializeComponent();
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

#if ANDROID
        this.Window.SetStatusBarAppearance(color: Colors.Crimson, lightElements: true);
        this.Window.SetNavigationBarAppearance(color: Colors.NavajoWhite, lightElements: false);
#endif
    }
}