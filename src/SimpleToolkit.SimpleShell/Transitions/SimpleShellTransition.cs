namespace SimpleToolkit.SimpleShell.Transitions
{
    // TODO: If a ShellContent is not in a ShellSection, transition is not played

    /// <summary>
    /// Type of a page transition in <see cref="SimpleShell"/>.
    /// </summary>
    public enum SimpleShellTransitionType
    {
        /// <summary>
        /// 
        /// </summary>
        Switching,
        /// <summary>
        /// 
        /// </summary>
        Pushing,
        /// <summary>
        /// 
        /// </summary>
        Popping
    }

    /// <summary>
    /// Arguments of a page transition in in <see cref="SimpleShell"/>.
    /// </summary>
    public class SimpleShellTransitionArgs
    {
        /// <summary>
        /// Page from which the navigation is initiated.
        /// </summary>
        public VisualElement OriginPage { get; protected set; }
        /// <summary>
        /// Destination page of the navigation.
        /// </summary>
        public VisualElement DestinationPage { get; protected set; }
        /// <summary>
        /// Progress of the transition.
        /// </summary>
        public double Progress { get; protected set; }
        /// <summary>
        /// Type of the transition.
        /// </summary>
        public SimpleShellTransitionType TransitionType { get; protected set; }

        public SimpleShellTransitionArgs(VisualElement originPage, VisualElement destinationPage, double progress, SimpleShellTransitionType transitionType)
        {
            OriginPage = originPage;
            DestinationPage = destinationPage;
            Progress = progress;
            TransitionType = transitionType;
        }
    }

    /// <summary>
    /// Page transition in <see cref="SimpleShell"/>.
    /// </summary>
    public class SimpleShellTransition
    {
        internal const uint DefaultDuration = 250;
        internal const bool DefaultDestinationPageAboveOnSwitching = true;
        internal const bool DefaultDestinationPageAboveOnPushing = true;
        internal const bool DefaultDestinationPageAboveOnPopping = true;

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
        /// Whether the destination page should be displayed above the origin page when switching root pages in the stack.
        /// </summary>
        public Func<SimpleShellTransitionArgs, bool> DestinationPageAboveOnSwitching { get; }
        /// <summary>
        /// Whether the destination page should be displayed above the origin page when pushing new page to the stack.
        /// </summary>
        public Func<SimpleShellTransitionArgs, bool> DestinationPageAboveOnPushing { get; }
        /// <summary>
        /// Whether the destination page should be displayed above the origin page when popping existing page from the stack.
        /// </summary>
        public Func<SimpleShellTransitionArgs, bool> DestinationPageAboveOnPopping { get; }

        /// <summary>
        /// Constructor that does not allow to dynamically set some properties of the transition.
        /// </summary>
        /// <param name="callback">Callback that is called when progress of the transition changes.</param>
        /// <param name="duration">Duration of the transition.</param>
        /// <param name="starting">Callback that is called when the transition starts.</param>
        /// <param name="finished">Callback that is called when the transition finishes.</param>
        /// <param name="destinationPageAboveOnSwitching">Whether the destination page should be displayed above the origin page when switching root pages in the stack.</param>
        /// <param name="destinationPageAboveOnPushing">Whether the destination page should be displayed above the origin page when pushing new page to the stack.</param>
        /// <param name="destinationPageAboveOnPopping">Whether the destination page should be displayed above the origin page when popping existing page from the stack.</param>
        public SimpleShellTransition(
            Action<SimpleShellTransitionArgs> callback,
            uint duration = DefaultDuration,
            Action<SimpleShellTransitionArgs> starting = null,
            Action<SimpleShellTransitionArgs> finished = null,
            bool destinationPageAboveOnSwitching = DefaultDestinationPageAboveOnSwitching,
            bool destinationPageAboveOnPushing = DefaultDestinationPageAboveOnPushing,
            bool destinationPageAboveOnPopping = DefaultDestinationPageAboveOnPopping)
        {
            Callback = callback;
            Duration = (args) => duration;
            Finished = finished;
            Starting = starting;
            DestinationPageAboveOnSwitching = (args) => destinationPageAboveOnSwitching;
            DestinationPageAboveOnPushing = (args) => destinationPageAboveOnPushing;
            DestinationPageAboveOnPopping = (args) => destinationPageAboveOnPopping;
        }

        /// <summary>
        /// Constructor that allows to dynamically set some properties of the transition.
        /// </summary>
        /// <param name="callback">Callback that is called when progress of the transition changes.</param>
        /// <param name="duration">Duration of the transition.</param>
        /// <param name="starting">Callback that is called when the transition starts.</param>
        /// <param name="finished">Callback that is called when the transition finishes.</param>
        /// <param name="destinationPageAboveOnSwitching">Whether the destination page should be displayed above the origin page when switching root pages in the stack.</param>
        /// <param name="destinationPageAboveOnPushing">Whether the destination page should be displayed above the origin page when pushing new page to the stack.</param>
        /// <param name="destinationPageAboveOnPopping">Whether the destination page should be displayed above the origin page when popping existing page from the stack.</param>
        public SimpleShellTransition(
            Action<SimpleShellTransitionArgs> callback,
            Func<SimpleShellTransitionArgs, uint> duration = null,
            Action<SimpleShellTransitionArgs> starting = null,
            Action<SimpleShellTransitionArgs> finished = null,
            Func<SimpleShellTransitionArgs, bool> destinationPageAboveOnSwitching = null,
            Func<SimpleShellTransitionArgs, bool> destinationPageAboveOnPushing = null,
            Func<SimpleShellTransitionArgs, bool> destinationPageAboveOnPopping = null)
        {
            Callback = callback;
            Duration = duration ?? ((args) => DefaultDuration);
            Finished = finished;
            Starting = starting;
            DestinationPageAboveOnSwitching = destinationPageAboveOnSwitching ?? ((args) => DefaultDestinationPageAboveOnSwitching);
            DestinationPageAboveOnPushing = destinationPageAboveOnPushing ?? ((args) => DefaultDestinationPageAboveOnPushing);
            DestinationPageAboveOnPopping = destinationPageAboveOnPopping ?? ((args) => DefaultDestinationPageAboveOnPopping);
        }
    }
}
