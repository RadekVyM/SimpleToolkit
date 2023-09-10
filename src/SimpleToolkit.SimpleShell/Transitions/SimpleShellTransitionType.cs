namespace SimpleToolkit.SimpleShell.Transitions;

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