using SimpleToolkit.SimpleShell.Playground.Views.Pages;
using SimpleToolkit.SimpleShell.Transitions;
using SimpleToolkit.SimpleShell.Extensions;

namespace SimpleToolkit.SimpleShell.Playground;

public partial class NoTabsAppShell : SimpleShell
{
	public NoTabsAppShell()
    {
        Routing.RegisterRoute(nameof(ImagePage), typeof(ImagePage));
        Routing.RegisterRoute(nameof(FirstYellowDetailPage), typeof(FirstYellowDetailPage));
        Routing.RegisterRoute(nameof(SecondYellowDetailPage), typeof(SecondYellowDetailPage));
        Routing.RegisterRoute(nameof(ThirdYellowDetailPage), typeof(ThirdYellowDetailPage));
        Routing.RegisterRoute(nameof(FourthYellowDetailPage), typeof(FourthYellowDetailPage));
        Routing.RegisterRoute(nameof(FirstGreenDetailPage), typeof(FirstGreenDetailPage));
        Routing.RegisterRoute(nameof(ImageDetailPage), typeof(ImageDetailPage));

        InitializeComponent();

        this.SetTransition(
            callback: static args =>
            {
                switch (args.TransitionType)
                {
                    case SimpleShellTransitionType.Switching:
                        if (args.OriginShellSectionContainer == args.DestinationShellSectionContainer)
                        {
                            // Navigatating within the same ShellSection
                            args.OriginPage.Opacity = 1 - args.Progress;
                            args.DestinationPage.Opacity = args.Progress;
                        }
                        else
                        {
                            // Navigatating between different ShellSections
                            (args.OriginShellSectionContainer ?? args.OriginPage).Opacity = 1 - args.Progress;
                            (args.DestinationShellSectionContainer ?? args.DestinationPage).Opacity = args.Progress;
                        }
                        break;
                    case SimpleShellTransitionType.Pushing:
                        // Hide the page until it is fully measured
                        args.DestinationPage.Opacity = args.DestinationPage.Width < 0 ? 0.01 : 1;
                        // Slide the page in from right
                        args.DestinationPage.TranslationX = (1 - args.Progress) * args.DestinationPage.Width;
                        break;
                    case SimpleShellTransitionType.Popping:
                        // Slide the page out to right
                        args.OriginPage.TranslationX = args.Progress * args.OriginPage.Width;
                        break;
                }
            },
            finished: static args =>
            {
                args.OriginPage.Opacity = 1;
                args.OriginPage.TranslationX = 0;
                args.DestinationPage.Opacity = 1;
                args.DestinationPage.TranslationX = 0;
                if (args.OriginShellSectionContainer is not null)
                    args.OriginShellSectionContainer.Opacity = 1;
                if (args.DestinationShellSectionContainer is not null)
                    args.DestinationShellSectionContainer.Opacity = 1;
            },
            destinationPageInFront: static args => args.TransitionType switch
            {
                SimpleShellTransitionType.Popping => false,
                _ => true
            },
            duration: static args => args.TransitionType switch
            {
                SimpleShellTransitionType.Switching => 300u,
                _ => 200u
            },
            easing: static args => args.TransitionType switch
            {
                SimpleShellTransitionType.Pushing => Easing.SinIn,
                SimpleShellTransitionType.Popping => Easing.SinOut,
                _ => Easing.Linear
            });
    }

    private async void ShellItemButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var shellItem = button.BindingContext as BaseShellItem;

        // Navigate to a new tab if it is not the current tab
        if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
            await GoToAsync($"///{shellItem.Route}", true);
    }

    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await GoToAsync("..");
    }
}
