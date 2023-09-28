using Playground.Core.Views.Pages;
using SimpleToolkit.SimpleShell;
using SimpleToolkit.SimpleShell.Transitions;
using SimpleToolkit.SimpleShell.Extensions;
using Playground.Core;

namespace Playground.ShellItems;

public partial class AppShell : SimpleShell
{
    public AppShell()
    {
        Routing.RegisterRoute(nameof(FirstYellowDetailPage), typeof(FirstYellowDetailPage));
        Routing.RegisterRoute(nameof(SecondYellowDetailPage), typeof(SecondYellowDetailPage));
        Routing.RegisterRoute(nameof(ThirdYellowDetailPage), typeof(ThirdYellowDetailPage));
        Routing.RegisterRoute(nameof(FourthYellowDetailPage), typeof(FourthYellowDetailPage));
        Routing.RegisterRoute(nameof(FirstGreenDetailPage), typeof(FirstGreenDetailPage));

        InitializeComponent();

        if (SimpleShell.UsesPlatformTransitions)
        {
            this.SetTransition(new PlatformSimpleShellTransition
            {
#if ANDROID
                SwitchingEnterAnimation = static (args) => Resource.Animation.simpleshell_enter_right,
                SwitchingLeaveAnimation = static (args) => Resource.Animation.simpleshell_exit_left,
                PushingEnterAnimation = static (args) => Resource.Animation.simpleshell_enter_right,
                PushingLeaveAnimation = static (args) => Resource.Animation.simpleshell_exit_left,
                PoppingEnterAnimation = static (args) => Resource.Animation.simpleshell_enter_left,
                PoppingLeaveAnimation = static (args) => Resource.Animation.simpleshell_exit_right,
#elif IOS || MACCATALYST
                SwitchingAnimation = static (args) => static (from, to) =>
                {
                    from.Alpha = 0;
                    to.Alpha = 1;
                },
                SwitchingAnimationStarting = static (args) => static (from, to) =>
                {
                    to.Alpha = 0;
                },
                SwitchingAnimationFinished = static (args) => static (from, to) =>
                {
                    from.Alpha = 1;
                }
#endif
            });
        }
        else
        {
            this.SetTransition(Transitions.DefaultCustomTransition);
        }
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);

        // Just to check if handlers are set to correct items
        var shellItemHandlers = Items.Select(i => i.Handler).ToArray();
        var shellSectionHandlers = ShellSections?.Select(i => i.Handler).ToArray();
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