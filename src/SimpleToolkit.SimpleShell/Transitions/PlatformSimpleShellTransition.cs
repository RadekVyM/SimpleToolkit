namespace SimpleToolkit.SimpleShell.Transitions;

/// <summary>
/// Platform-specific page transition in <see cref="SimpleShell"/>.
/// </summary>
public class PlatformSimpleShellTransition : ISimpleShellTransition
{
#if ANDROID

    /// <summary>
    /// Whether the destination page should be displayed in front of the origin page on page switching.
    /// </summary>
    public Func<SimpleShellTransitionArgs, bool> DestinationPageInFrontOnSwitching { get; init; }
    /// <summary>
    /// Whether the destination page should be displayed in front of the origin page on page pushing.
    /// </summary>
    public Func<SimpleShellTransitionArgs, bool> DestinationPageInFrontOnPushing { get; init; }
    /// <summary>
    /// Whether the destination page should be displayed in front of the origin page on page popping.
    /// </summary>
    public Func<SimpleShellTransitionArgs, bool> DestinationPageInFrontOnPopping { get; init; }
    /// <summary>
    /// ID of an animation that is applied to the entering page on page switching.
    /// </summary>
    public Func<SimpleShellTransitionArgs, int> SwitchingEnterAnimation { get; init; }
    /// <summary>
    /// ID of an animation that is applied to the leaving page on page switching.
    /// </summary>
    public Func<SimpleShellTransitionArgs, int> SwitchingLeaveAnimation { get; init; }
    /// <summary>
    /// ID of an animation or animator that is applied to the entering page on page pushing.
    /// </summary>
    public Func<SimpleShellTransitionArgs, int> PushingEnterAnimation { get; init; }
    /// <summary>
    /// ID of an animation or animator that is applied to the leaving page on page pushing.
    /// </summary>
    public Func<SimpleShellTransitionArgs, int> PushingLeaveAnimation { get; init; }
    /// <summary>
    /// ID of an animation or animator that is applied to the entering page on page popping.
    /// </summary>
    public Func<SimpleShellTransitionArgs, int> PoppingEnterAnimation { get; init; }
    /// <summary>
    /// ID of an animation or animator that is applied to the leaving page on page popping.
    /// </summary>
    public Func<SimpleShellTransitionArgs, int> PoppingLeaveAnimation { get; init; }

#elif IOS || MACCATALYST

    /// <summary>
    /// Delegate for switching animation actions.
    /// </summary>
    /// <param name="from">The origin page platform view</param>
    /// <param name="to">The destination page platform view</param>
    public delegate void TransitionAnimation(UIKit.UIView from, UIKit.UIView to);

    /// <summary>
    /// Whether the destination page should be displayed in front of the origin page on page switching.
    /// </summary>
    public Func<SimpleShellTransitionArgs, bool> DestinationPageInFrontOnSwitching { get; init; }
    /// <summary>
    /// Duration of the animation on page switching.
    /// </summary>
    public Func<SimpleShellTransitionArgs, double> SwitchingAnimationDuration { get; init; }
    /// <summary>
    /// Switching animation represented by an action with two parameters for platform views (of type <see cref="UIKit.UIView"/>) of the origin and destination pages. This is where you change any animatable properties of the platform views. The change will be automatically animated.
    /// </summary>
    public Func<SimpleShellTransitionArgs, TransitionAnimation> SwitchingAnimation { get; init; }
    /// <summary>
    /// Action which is called before the animation starts. All preparatory work (such as setting initial values of the animated properties) should be done in this action.
    /// </summary>
    public Func<SimpleShellTransitionArgs, TransitionAnimation> SwitchingAnimationStarting { get; init; }
    /// <summary>
    /// Action which is called right after the animation plays. All cleaning work (such as setting the values of the animated properties back to initial values) should be done in this action.
    /// </summary>
    public Func<SimpleShellTransitionArgs, TransitionAnimation> SwitchingAnimationFinished { get; init; }
    /// <summary>
    /// Object of type <see cref="UIKit.IUIViewControllerAnimatedTransitioning"/>, which represents the animation that is played on page pushing.
    /// </summary>
    public Func<SimpleShellTransitionArgs, UIKit.IUIViewControllerAnimatedTransitioning> PushingAnimation { get; init; }
    /// <summary>
    /// Object of type <see cref="UIKit.IUIViewControllerAnimatedTransitioning"/>, which represents the animation that is played on page popping.
    /// </summary>
    public Func<SimpleShellTransitionArgs, UIKit.IUIViewControllerAnimatedTransitioning> PoppingAnimation { get; init; }

#elif WINDOWS

    /// <summary>
    /// <see cref="Microsoft.UI.Xaml.Media.Animation.EntranceThemeTransition"/> object that is applied on page switching.
    /// </summary>
    public Func<SimpleShellTransitionArgs, Microsoft.UI.Xaml.Media.Animation.EntranceThemeTransition> SwitchingAnimation { get; init; }
    /// <summary>
    /// <see cref="Microsoft.UI.Xaml.Media.Animation.NavigationTransitionInfo"/> object that is applied on page pushing.
    /// </summary>
    public Func<SimpleShellTransitionArgs, Microsoft.UI.Xaml.Media.Animation.NavigationTransitionInfo> PushingAnimation { get; init; }
    /// <summary>
    /// <see cref="Microsoft.UI.Xaml.Media.Animation.NavigationTransitionInfo"/> object that is applied on page popping.
    /// </summary>
    public Func<SimpleShellTransitionArgs, Microsoft.UI.Xaml.Media.Animation.NavigationTransitionInfo> PoppingAnimation { get; init; }

#endif
}