using SimpleToolkit.SimpleShell.Transitions;

namespace SimpleToolkit.SimpleShell.Playground.Views.Pages;

public partial class ImagePage : ContentPage
{
	public ImagePage()
	{
		InitializeComponent();

        SimpleShell.SetTransition(this, new SimpleShellTransition(args =>
        {
            args.OriginPage.TranslationY = args.Progress * (-args.OriginPage.Height);
            args.DestinationPage.TranslationY = (1 - args.Progress) * (args.DestinationPage.Height);
        },
        500,
        finished: args =>
        {
            args.OriginPage.TranslationY = 0;
            args.DestinationPage.TranslationY = 0;
        }));
	}

    //protected override void OnNavigatedTo(NavigatedToEventArgs args)
    //{
    //    base.OnNavigatedTo(args);

    //    this.Window.SubscribeToSafeAreaChanges(OnSafeAreaChanged);
    //}

    //private void OnSafeAreaChanged(Thickness safeAreaPadding)
    //{
    //    this.Padding = safeAreaPadding;
    //}

    //protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    //{
    //    base.OnNavigatedFrom(args);

    //    this.Window.UnsubscribeFromSafeAreaChanges(OnSafeAreaChanged);
    //}
}