using SimpleToolkit.SimpleShell;
using SimpleToolkit.SimpleShell.Extensions;
using SimpleToolkit.SimpleShell.Transitions;

namespace Playground.Original.Views.Pages;

public partial class ImagePage : ContentPage
{
	public ImagePage()
	{
		InitializeComponent();

        this.SetTransition(new SimpleShellTransition(
            callback: static args => args.DestinationPage.Scale = args.Progress,
            starting: static args => args.DestinationPage.Scale = 0,
            finished: static args => args.DestinationPage.Scale = 1,
            duration: static args => 500u,
            easing: static args => Easing.SpringOut)
            .CombinedWith(
                transition: SimpleShell.Current.GetTransition(),
                when: static args => args.TransitionType != SimpleShellTransitionType.Pushing));
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