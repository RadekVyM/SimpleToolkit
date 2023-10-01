namespace SimpleToolkit.SimpleShell.Transitions;

public class PlatformSimpleShellTransition : ISimpleShellTransition
{
#if ANDROID
    public Func<SimpleShellTransitionArgs, bool> DestinationPageInFrontOnSwitching { get; init; }
    public Func<SimpleShellTransitionArgs, bool> DestinationPageInFrontOnPushing { get; init; }
    public Func<SimpleShellTransitionArgs, bool> DestinationPageInFrontOnPopping { get; init; }
    public Func<SimpleShellTransitionArgs, int> SwitchingEnterAnimation { get; init; }
    public Func<SimpleShellTransitionArgs, int> SwitchingLeaveAnimation { get; init; }
    public Func<SimpleShellTransitionArgs, int> PushingEnterAnimation { get; init; }
    public Func<SimpleShellTransitionArgs, int> PushingLeaveAnimation { get; init; }
    public Func<SimpleShellTransitionArgs, int> PoppingEnterAnimation { get; init; }
    public Func<SimpleShellTransitionArgs, int> PoppingLeaveAnimation { get; init; }

    public PlatformSimpleShellTransition()
	{
	}

#elif IOS || MACCATALYST
    public delegate void TransitionAnimation(UIKit.UIView from, UIKit.UIView to);

    public Func<SimpleShellTransitionArgs, bool> DestinationPageInFrontOnSwitching { get; init; }
    public Func<SimpleShellTransitionArgs, double> SwitchingAnimationDuration { get; init; }
    public Func<SimpleShellTransitionArgs, TransitionAnimation> SwitchingAnimation { get; init; }
    public Func<SimpleShellTransitionArgs, TransitionAnimation> SwitchingAnimationStarting { get; init; }
    public Func<SimpleShellTransitionArgs, TransitionAnimation> SwitchingAnimationFinished { get; init; }
    public Func<UIKit.IUIViewControllerAnimatedTransitioning> PushingAnimation { get; init; }
    public Func<UIKit.IUIViewControllerAnimatedTransitioning> PoppingAnimation { get; init; }

    public PlatformSimpleShellTransition()
	{
	}

#elif WINDOWS
    public Func<SimpleShellTransitionArgs, Microsoft.UI.Xaml.Media.Animation.EntranceThemeTransition> SwitchingAnimation { get; init; }
    public Func<SimpleShellTransitionArgs, Microsoft.UI.Xaml.Media.Animation.NavigationTransitionInfo> PushingAnimation { get; init; }
    public Func<SimpleShellTransitionArgs, Microsoft.UI.Xaml.Media.Animation.NavigationTransitionInfo> PoppingAnimation { get; init; }

	public PlatformSimpleShellTransition()
    {
    }
#endif
}