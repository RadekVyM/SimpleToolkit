namespace SimpleToolkit.SimpleShell.Transitions;

/// <summary>
/// Arguments of a transition during navigation between two pages in <see cref="SimpleShell"/>.
/// </summary>
public class SimpleShellTransitionArgs(
    VisualElement originPage,
    VisualElement destinationPage,
    VisualElement? originShellSectionContainer,
    VisualElement? destinationShellSectionContainer,
    VisualElement? originShellItemContainer,
    VisualElement? destinationShellItemContainer,
    SimpleShell shell,
    double progress,
    SimpleShellTransitionType transitionType,
    bool isOriginPageRoot,
    bool isDestinationPageRoot
) {
    /// <summary>
    /// Page from which the navigation is initiated.
    /// </summary>
    public VisualElement OriginPage { get; protected set; } = originPage;
    /// <summary>
    /// Destination page of the navigation.
    /// </summary>
    public VisualElement DestinationPage { get; protected set; } = destinationPage;
    /// <summary>
    /// <see cref="ShellSection"/> container from which the navigation is initiated.
    /// </summary>
    public VisualElement? OriginShellSectionContainer { get; protected set; } = originShellSectionContainer;
    /// <summary>
    /// Destination <see cref="ShellSection"/> container of the navigation.
    /// </summary>
    public VisualElement? DestinationShellSectionContainer { get; protected set; } = destinationShellSectionContainer;
    /// <summary>
    /// <see cref="ShellItem"/> container from which the navigation is initiated.
    /// </summary>
    public VisualElement? OriginShellItemContainer { get; protected set; } = originShellItemContainer;
    /// <summary>
    /// Destination <see cref="ShellItem"/> container of the navigation.
    /// </summary>
    public VisualElement? DestinationShellItemContainer { get; protected set; } = destinationShellItemContainer;
    /// <summary>
    /// Current <see cref="SimpleShell"/> instance.
    /// </summary>
    public SimpleShell Shell { get; protected set; } = shell;
    /// <summary>
    /// Progress of the transition. Number between 0 and 1.
    /// </summary>
    public double Progress { get; protected set; } = progress;
    /// <summary>
    /// Type of the transition.
    /// </summary>
    public SimpleShellTransitionType TransitionType { get; protected set; } = transitionType;
    /// <summary>
    /// Whether the origin page is a root page.
    /// </summary>
    public bool IsOriginPageRoot { get; protected set; } = isOriginPageRoot;
    /// <summary>
    /// Whether the destination page is a root page.
    /// </summary>
    public bool IsDestinationPageRoot { get; protected set; } = isDestinationPageRoot;
}