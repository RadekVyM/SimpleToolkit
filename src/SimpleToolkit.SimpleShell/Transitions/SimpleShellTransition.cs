namespace SimpleToolkit.SimpleShell.Transitions
{
    // TODO: If a ShellContent is not in a ShellSection, transition is not played

    public enum SimpleShellTransitionType
    {
        Switching, Pushing, Popping
    }

    public class SimpleShellTransitionArgs
    {
        public VisualElement OriginPage { get; protected set; }
        public VisualElement DestinationPage { get; protected set; }
        public double Progress { get; protected set; }
        public SimpleShellTransitionType TransitionType { get; protected set; }


        public SimpleShellTransitionArgs(VisualElement originPage, VisualElement destinationPage, double progress, SimpleShellTransitionType transitionType)
        {
            OriginPage = originPage;
            DestinationPage = destinationPage;
            Progress = progress;
            TransitionType = transitionType;
        }
    }

    public class SimpleShellTransition
    {
        internal const uint DefaultDuration = 250;
        internal const bool DefaultDestinationPageAboveOnSwitching = true;
        internal const bool DefaultDestinationPageAboveOnPushing = true;
        internal const bool DefaultDestinationPageAboveOnPopping = true;

        public Action<SimpleShellTransitionArgs> Callback { get; }
        public Action<SimpleShellTransitionArgs> Starting { get; }
        public Action<SimpleShellTransitionArgs> Finished { get; }
        public Func<SimpleShellTransitionArgs, uint> Duration { get; }
        public Func<SimpleShellTransitionArgs, bool> DestinationPageAboveOnSwitching { get; }
        public Func<SimpleShellTransitionArgs, bool> DestinationPageAboveOnPushing { get; }
        public Func<SimpleShellTransitionArgs, bool> DestinationPageAboveOnPopping { get; }

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
