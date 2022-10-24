using SimpleToolkit.Core;
using SimpleToolkit.SimpleShell.Extensions;
using SimpleToolkit.SimpleShell.Playground.Views.Pages;
using SimpleToolkit.SimpleShell.Transitions;

namespace SimpleToolkit.SimpleShell.Playground;

public partial class SampleAppShell : SimpleShell
{
	public SampleAppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(ImagePage), typeof(ImagePage));

        this.SetTransition(
            callback: args =>
            {
                switch (args.TransitionType)
                {
                    case SimpleShellTransitionType.Switching:
                        args.OriginPage.Opacity = 1 - args.Progress;
                        args.DestinationPage.Opacity = args.Progress;
                        break;
                    case SimpleShellTransitionType.Pushing:
                        args.DestinationPage.TranslationX = (1 - args.Progress) * args.DestinationPage.Width;
                        break;
                    case SimpleShellTransitionType.Popping:
                        args.OriginPage.TranslationX = args.Progress * args.OriginPage.Width;
                        break;
                }
            },
            finished: args =>
            {
                args.OriginPage.Opacity = 1;
                args.OriginPage.TranslationX = 1;
                args.DestinationPage.Opacity = 1;
                args.DestinationPage.TranslationX = 1;
            },
            destinationPageInFront: args => args.TransitionType switch {
                SimpleShellTransitionType.Popping => false,
                _ => true
            },
            duration: args => args.TransitionType switch {
                SimpleShellTransitionType.Switching => 500,
                _ => 350
            });

        Loaded += SimpleAppShellLoaded;
    }

    private void SimpleAppShellLoaded(object sender, EventArgs e)
    {
        this.Window.SubscribeToSafeAreaChanges(OnSafeAreaChanged);
    }

    private void OnSafeAreaChanged(Thickness safeAreaPadding)
    {
        rootContainer.Padding = safeAreaPadding;
    }

    private async void ShellItemButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var shellItem = button.BindingContext as BaseShellItem;

        if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
            await GoToAsync($"///{shellItem.Route}", true);
    }

    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await GoToAsync("..");
    }
}