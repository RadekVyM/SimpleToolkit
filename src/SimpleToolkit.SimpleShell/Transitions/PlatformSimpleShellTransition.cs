namespace SimpleToolkit.SimpleShell.Transitions;

public class PlatformSimpleShellTransition
{
#if ANDROID
    public Func<SimpleShellTransitionArgs, bool> SwitchingDestinationPageInFront { get; init; }
    public Func<SimpleShellTransitionArgs, int?> SwitchingEnterAnimation { get; init; }
    public Func<SimpleShellTransitionArgs, int?> SwitchingLeaveAnimation { get; init; }
    public Func<SimpleShellTransitionArgs, int?> PushingEnterAnimation { get; init; }
    public Func<SimpleShellTransitionArgs, int?> PushingLeaveAnimation { get; init; }
    public Func<SimpleShellTransitionArgs, int?> PoppingEnterAnimation { get; init; }
    public Func<SimpleShellTransitionArgs, int?> PoppingLeaveAnimation { get; init; }

    public PlatformSimpleShellTransition()
	{
	}
#elif IOS || MACCATALYST
	public PlatformSimpleShellTransition()
	{
	}
#elif WINDOWS
	public PlatformSimpleShellTransition()
	{
	}
#endif
}