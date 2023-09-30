using Playground.Core.Views.Pages;
using SimpleToolkit.SimpleShell;
using SimpleToolkit.SimpleShell.Transitions;
using SimpleToolkit.SimpleShell.Extensions;
using Playground.Core;
#if IOS || MACCATALYST
using CoreGraphics;
using Foundation;
#endif

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
                SwitchingAnimationDuration = static (args) => 0.6,
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
                },
                PushingAnimation = static () => new AppleTransition(true),
                PoppingAnimation = static () => new AppleTransition(false),
#elif WINDOWS
                SwitchingAnimation = static (args) => new Microsoft.UI.Xaml.Media.Animation.EntranceThemeTransition { FromVerticalOffset = -50 },
                PushingAnimation = static (args) => new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo(),
                PoppingAnimation = static (args) => new Microsoft.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo(),
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

#if IOS || MACCATALYST

public class AppleTransition : NSObject, UIKit.IUIViewControllerAnimatedTransitioning
{
    private readonly bool pushing;

    public AppleTransition(bool pushing)
    {
        this.pushing = pushing;
    }

    public void AnimateTransition(UIKit.IUIViewControllerContextTransitioning transitionContext)
    {
        var fromView = transitionContext.GetViewFor(UIKit.UITransitionContext.FromViewKey);
        var toView = transitionContext.GetViewFor(UIKit.UITransitionContext.ToViewKey);
        var duration = TransitionDuration(transitionContext);

        var container = transitionContext.ContainerView;

        if (pushing)
            container.AddSubview(toView);
        else
            container.InsertSubviewBelow(toView, fromView);

        var toViewFrame = toView.Frame;
        toView.Frame = new CGRect(x: pushing ? toView.Frame.Width : -toView.Frame.Width, y: toView.Frame.Location.Y, width: toView.Frame.Width, height: toView.Frame.Height);

        var animations = () =>
        {
            UIKit.UIView.AddKeyframeWithRelativeStartTime(0.0, 0.5, () =>
            {
                toView.Alpha = 1;
                if (pushing)
                    fromView.Alpha = 0;
            });
            UIKit.UIView.AddKeyframeWithRelativeStartTime(0.0, 1, () =>
            {
                toView.Frame = toViewFrame;
                fromView.Frame = new CGRect(x: pushing ? -fromView.Frame.Width : fromView.Frame.Width, y: fromView.Frame.Location.Y, width: fromView.Frame.Width, height: fromView.Frame.Height);
                if (!pushing)
                    fromView.Alpha = 0;
            });
        };

        UIKit.UIView.AnimateKeyframes(
            duration,
            0,
            UIKit.UIViewKeyframeAnimationOptions.CalculationModeCubic,
            animations,
            (finished) =>
            {
                container.AddSubview(toView);
                transitionContext.CompleteTransition(!transitionContext.TransitionWasCancelled);
                fromView.Alpha = 1;
                toView.Alpha = 1;
            });
    }

    public double TransitionDuration(UIKit.IUIViewControllerContextTransitioning transitionContext)
    {
        return 0.3;
    }
}

#endif