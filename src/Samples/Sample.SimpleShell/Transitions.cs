using SimpleToolkit.SimpleShell.Transitions;

namespace Sample.SimpleShell;

public static class Transitions
{
    public static PlatformSimpleShellTransition CustomPlatformTransition => new PlatformSimpleShellTransition
    {
#if ANDROID
        SwitchingEnterAnimation = static (args) => Resource.Animation.nav_default_enter_anim,
        SwitchingLeaveAnimation = static (args) => Resource.Animation.nav_default_exit_anim,
        PushingEnterAnimation = static (args) => Resource.Animator.flip_right_in,
        PushingLeaveAnimation = static (args) => Resource.Animator.flip_right_out,
        PoppingEnterAnimation = static (args) => Resource.Animator.flip_left_in,
        PoppingLeaveAnimation = static (args) => Resource.Animator.flip_left_out,
#elif IOS || MACCATALYST
        SwitchingAnimationDuration = static (args) => 0.3,
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
        PushingAnimation = static (args) => new AppleTransitioning(true),
        PoppingAnimation = static (args) => new AppleTransitioning(false),
#elif WINDOWS
        PushingAnimation = static (args) => new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo(),
        PoppingAnimation = static (args) => new Microsoft.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo(),
#endif
    };
}

#if IOS || MACCATALYST

public class AppleTransitioning : Foundation.NSObject, UIKit.IUIViewControllerAnimatedTransitioning
{
    private readonly bool pushing;

    public AppleTransitioning(bool pushing)
    {
        this.pushing = pushing;
    }

    public void AnimateTransition(UIKit.IUIViewControllerContextTransitioning transitionContext)
    {
        var fromView = transitionContext.GetViewFor(UIKit.UITransitionContext.FromViewKey);
        var toView = transitionContext.GetViewFor(UIKit.UITransitionContext.ToViewKey);
        var container = transitionContext.ContainerView;
        var duration = TransitionDuration(transitionContext);

        if (pushing)
            container.AddSubview(toView);
        else
            container.InsertSubviewBelow(toView, fromView);

        var currentFrame = pushing ? toView.Frame : fromView.Frame;
        var offsetFrame = new CoreGraphics.CGRect(x: currentFrame.Location.X, y: currentFrame.Height, width: currentFrame.Width, height: currentFrame.Height);

        if (pushing)
        {
            toView.Frame = offsetFrame;
            toView.Alpha = 0;
        }
             
        var animations = () =>
        {
            UIKit.UIView.AddKeyframeWithRelativeStartTime(0.0, 1, () =>
            {
                if (pushing)
                {
                    toView.Frame = currentFrame;
                    toView.Alpha = 1;
                }
                else
                {
                    fromView.Frame = offsetFrame;
                    fromView.Alpha = 0;
                }
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

                toView.Alpha = 1;
                fromView.Alpha = 1;
            });
    }

    public double TransitionDuration(UIKit.IUIViewControllerContextTransitioning transitionContext)
    {
        return 0.25;
    }
}

#endif