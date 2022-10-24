using SimpleToolkit.SimpleShell.Extensions;

namespace SimpleToolkit.SimpleShell.Playground.Views.Pages;

public partial class ImagePage : ContentPage
{
	public ImagePage()
	{
		InitializeComponent();

        this.SetTransition(
            callback: args =>
            {
                args.DestinationPage.Scale = args.Progress;
            },
            500,
            finished: args =>
            {
                args.DestinationPage.Scale = 1;
            });
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