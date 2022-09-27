namespace SimpleToolkit.SimpleShell.Transitions
{
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
        public Action<SimpleShellTransitionArgs> Callback { get; }
        public Action<SimpleShellTransitionArgs> Starting { get; }
        public Action<SimpleShellTransitionArgs> Finished { get; }
        public uint Duration { get; }
        public bool DestinationPageAboveOnSwitching { get; }
        public bool DestinationPageAboveOnPushing { get; }
        public bool DestinationPageAboveOnPopping { get; }

        public SimpleShellTransition(
            Action<SimpleShellTransitionArgs> callback,
            uint duration = 250,
            Action<SimpleShellTransitionArgs> starting = null,
            Action<SimpleShellTransitionArgs> finished = null,
            bool destinationPageAboveOnSwitching = true,
            bool destinationPageAboveOnPushing = true,
            bool destinationPageAboveOnPopping = true)
        {
            Callback = callback;
            Duration = duration;
            Finished = finished;
            Starting = starting;
            DestinationPageAboveOnSwitching = destinationPageAboveOnSwitching;
            DestinationPageAboveOnPushing = destinationPageAboveOnPushing;
            DestinationPageAboveOnPopping = destinationPageAboveOnPopping;
        }
    }
}
