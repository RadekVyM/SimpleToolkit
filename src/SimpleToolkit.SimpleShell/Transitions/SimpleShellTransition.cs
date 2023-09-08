namespace SimpleToolkit.SimpleShell.Transitions
{
    /// <summary>
    /// Type of a page transition in <see cref="SimpleShell"/>.
    /// </summary>
    public enum SimpleShellTransitionType
    {
        /// <summary>
        /// New root page is being set.
        /// </summary>
        Switching,
        /// <summary>
        /// New page is being pushed to the navigation stack.
        /// </summary>
        Pushing,
        /// <summary>
        /// Existing page is being popped from the navigation stack.
        /// </summary>
        Popping
    }

    /// <summary>
    /// Arguments of a transition during navigation between two pages in <see cref="SimpleShell"/>.
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
        /// <see cref="ShellSection"/> container from which the navigation is initiated.
        /// </summary>
        public VisualElement OriginShellSectionContainer { get; protected set; }
        /// <summary>
        /// Destination <see cref="ShellSection"/> container of the navigation.
        /// </summary>
        public VisualElement DestinationShellSectionContainer { get; protected set; }
        /// <summary>
        /// <see cref="ShellItem"/> container from which the navigation is initiated.
        /// </summary>
        public VisualElement OriginShellItemContainer { get; protected set; }
        /// <summary>
        /// Destination <see cref="ShellItem"/> container of the navigation.
        /// </summary>
        public VisualElement DestinationShellItemContainer { get; protected set; }
        /// <summary>
        /// Current <see cref="SimpleShell"/> instance.
        /// </summary>
        public SimpleShell Shell { get; protected set; }
        /// <summary>
        /// Progress of the transition. Number between 0 and 1.
        /// </summary>
        public double Progress { get; protected set; }
        /// <summary>
        /// Type of the transition.
        /// </summary>
        public SimpleShellTransitionType TransitionType { get; protected set; }
        /// <summary>
        /// Whether the origin page is a root page.
        /// </summary>
        public bool IsOriginPageRoot { get; protected set; }
        /// <summary>
        /// Whether the destination page is a root page.
        /// </summary>
        public bool IsDestinationPageRoot { get; protected set; }

        public SimpleShellTransitionArgs(
            VisualElement originPage,
            VisualElement destinationPage,
            VisualElement originShellSectionContainer,
            VisualElement destinationShellSectionContainer,
            VisualElement originShellItemContainer,
            VisualElement destinationShellItemContainer,
            SimpleShell shell,
            double progress,
            SimpleShellTransitionType transitionType,
            bool isOriginPageRoot,
            bool isDestinationPageRoot)
        {
            OriginPage = originPage;
            DestinationPage = destinationPage;
            OriginShellSectionContainer = originShellSectionContainer;
            DestinationShellSectionContainer = destinationShellSectionContainer;
            OriginShellItemContainer = originShellItemContainer;
            DestinationShellItemContainer = destinationShellItemContainer;
            Shell = shell;
            Progress = progress;
            TransitionType = transitionType;
            IsOriginPageRoot = isOriginPageRoot;
            IsDestinationPageRoot = isDestinationPageRoot;
        }
    }

    /// <summary>
    /// Page transition in <see cref="SimpleShell"/>.
    /// </summary>
    public class SimpleShellTransition
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
}
