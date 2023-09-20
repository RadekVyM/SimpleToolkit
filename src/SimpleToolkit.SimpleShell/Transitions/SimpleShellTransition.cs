namespace SimpleToolkit.SimpleShell.Transitions;

/// <summary>
/// Page transition in <see cref="SimpleShell"/>.
/// </summary>
public class SimpleShellTransition : ISimpleShellTransition
{
    internal const uint DefaultDuration = 250;
    internal const bool DefaultDestinationPageInFrontOnSwitching = true;
    internal const bool DefaultDestinationPageInFrontOnPushing = true;
    internal const bool DefaultDestinationPageInFrontOnPopping = false;

    /// <summary>
    /// Callback that is called when progress of the transition changes.
    /// </summary>
    public Action<SimpleShellTransitionArgs> Callback { get; }
    /// <summary>
    /// Callback that is called when the transition starts.
    /// </summary>
    public Action<SimpleShellTransitionArgs> Starting { get; }
    /// <summary>
    /// Callback that is called when the transition finishes.
    /// </summary>
    public Action<SimpleShellTransitionArgs> Finished { get; }
    /// <summary>
    /// Duration of the transition.
    /// </summary>
    public Func<SimpleShellTransitionArgs, uint> Duration { get; }
    /// <summary>
    /// Whether the destination page should be displayed in front of the origin page when transitioning from one page to another.
    /// </summary>
    public Func<SimpleShellTransitionArgs, bool> DestinationPageInFront { get; }
    /// <summary>
    /// Easing of the transition animation.
    /// </summary>
    public Func<SimpleShellTransitionArgs, Easing> Easing { get; }

    /// <summary>
    /// Constructor that allows to dynamically set properties of the transition.
    /// </summary>
    /// <param name="callback">Callback that is called when progress of the transition changes.</param>
    /// <param name="duration">Duration of the transition.</param>
    /// <param name="starting">Callback that is called when the transition starts.</param>
    /// <param name="finished">Callback that is called when the transition finishes.</param>
    /// <param name="destinationPageInFront">Whether the destination page should be displayed in front of the origin page when transitioning from one page to another.</param>
    /// <param name="easing">Easing of the transition animation.</param>
    public SimpleShellTransition(
        Action<SimpleShellTransitionArgs> callback,
        Func<SimpleShellTransitionArgs, uint> duration = null,
        Action<SimpleShellTransitionArgs> starting = null,
        Action<SimpleShellTransitionArgs> finished = null,
        Func<SimpleShellTransitionArgs, bool> destinationPageInFront = null,
        Func<SimpleShellTransitionArgs, Easing> easing = null)
    {
        Callback = callback;
        Duration = duration ?? ((args) => DefaultDuration);
        Finished = finished;
        Starting = starting;
        DestinationPageInFront = destinationPageInFront ?? ((args) => args.TransitionType switch
        {
            SimpleShellTransitionType.Switching => DefaultDestinationPageInFrontOnSwitching,
            SimpleShellTransitionType.Pushing => DefaultDestinationPageInFrontOnPushing,
            SimpleShellTransitionType.Popping => DefaultDestinationPageInFrontOnPopping,
            _ => true
        });
        Easing = easing ?? ((args) => Microsoft.Maui.Easing.Linear);
    }
}