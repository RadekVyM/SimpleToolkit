namespace SimpleToolkit.SimpleShell.Transitions;

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