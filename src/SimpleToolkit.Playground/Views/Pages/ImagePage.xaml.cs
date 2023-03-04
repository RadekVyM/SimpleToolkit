using SimpleToolkit.SimpleShell.Extensions;
using SimpleToolkit.SimpleShell.Transitions;

namespace SimpleToolkit.SimpleShell.Playground.Views.Pages;

public partial class ImagePage : ContentPage
{
	public ImagePage()
	{
		InitializeComponent();

        this.SetTransition(new SimpleShellTransition(
            callback: args => args.DestinationPage.Scale = args.Progress,
            starting: args => args.DestinationPage.Scale = 0,
            finished: args => args.DestinationPage.Scale = 1,
            duration: args => 500u)
            .CombinedWith(transition: SimpleShell.Current.GetTransition(), when: args => args.TransitionType != SimpleShellTransitionType.Pushing));
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