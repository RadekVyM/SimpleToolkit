using SimpleToolkit.SimpleShell.Transitions;

namespace Playground.Original;

public static class Transitions
{
	public static SimpleShellTransition DefaultUniversalTransition { get; } = new SimpleShellTransition(
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

    public static PlatformSimpleShellTransition CustomPlatformTransition { get; } = new PlatformSimpleShellTransition
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
        PushingAnimation = static (args) => new AppleTransition(true),
        PoppingAnimation = static (args) => new AppleTransition(false),
#elif WINDOWS
        SwitchingAnimation = static (args) => new Microsoft.UI.Xaml.Media.Animation.EntranceThemeTransition { FromVerticalOffset = -50 },
        PushingAnimation = static (args) => new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo(),
        PoppingAnimation = static (args) => new Microsoft.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo(),
#endif
    };
}

#if IOS || MACCATALYST

public class AppleTransition : Foundation.NSObject, UIKit.IUIViewControllerAnimatedTransitioning
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
        toView.Frame = new CoreGraphics.CGRect(x: pushing ? toView.Frame.Width : -toView.Frame.Width, y: toView.Frame.Location.Y, width: toView.Frame.Width, height: toView.Frame.Height);

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
                fromView.Frame = new CoreGraphics.CGRect(x: pushing ? -fromView.Frame.Width : fromView.Frame.Width, y: fromView.Frame.Location.Y, width: fromView.Frame.Width, height: fromView.Frame.Height);
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