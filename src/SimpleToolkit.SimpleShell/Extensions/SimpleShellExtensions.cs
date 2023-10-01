using SimpleToolkit.SimpleShell.Transitions;

namespace SimpleToolkit.SimpleShell.Extensions;

/// <summary>
/// Extension methods for setting page transitions to <see cref="SimpleShell"/> pages.
/// </summary>
public static class SimpleShellExtensions
{
    /// <summary>
    /// Gets the page transition.
    /// </summary>
    /// <param name="page">Page with required transition.</param>
    public static ISimpleShellTransition GetTransition(this Page page)
    {
        return SimpleShell.GetTransition(page);
    }

    /// <summary>
    /// Sets the page transition.
    /// </summary>
    /// <param name="page">Destination page of a navigation with this transition.</param>
    /// <param name="transition">Transition of a navigation to the page.</param>
    public static void SetTransition(this Page page, ISimpleShellTransition transition)
    {
        SimpleShell.SetTransition(page, transition);
    }

    /// <summary>
    /// Creates and sets a new transition to the page.
    /// </summary>
    /// <param name="page">Destination page of a navigation with this transition.</param>
    /// <param name="callback">Callback that is called when progress of the transition changes.</param>
    /// <param name="duration">Duration of the transition.</param>
    /// <param name="starting">Callback that is called when the transition starts.</param>
    /// <param name="finished">Callback that is called when the transition finishes.</param>
    /// <param name="destinationPageInFront">Whether the destination page should be displayed in front of the origin page when transitioning from one page to another.</param>
    /// <param name="easing">Easing of the transition animation.</param>
    public static void SetTransition(
        this Page page,
        Action<SimpleShellTransitionArgs> callback,
        Func<SimpleShellTransitionArgs, uint> duration = null,
        Action<SimpleShellTransitionArgs> starting = null,
        Action<SimpleShellTransitionArgs> finished = null,
        Func<SimpleShellTransitionArgs, bool> destinationPageInFront = null,
        Func<SimpleShellTransitionArgs, Easing> easing = null)
    {
        page.SetTransition(new SimpleShellTransition(callback, duration, starting, finished, destinationPageInFront, easing));
    }

    /// <summary>
    /// Creates and sets a new transition to the page.
    /// </summary>
    /// <param name="page">Destination page of a navigation with this transition.</param>
    /// <param name="switchingCallback">Callback that is called when progress of the switching transition changes.</param>
    /// <param name="pushingCallback">Callback that is called when progress of the pushing transition changes.</param>
    /// <param name="poppingCallback">Callback that is called when progress of the popping transition changes.</param>
    /// <param name="duration">Duration of the transition.</param>
    /// <param name="starting">Callback that is called when the transition starts.</param>
    /// <param name="finished">Callback that is called when the transition finishes.</param>
    /// <param name="destinationPageInFront">Whether the destination page should be displayed in front of the origin page when transitioning from one page to another.</param>
    /// <param name="easing">Easing of the transition animation.</param>
    public static void SetTransition(
        this Page page,
        Action<SimpleShellTransitionArgs> switchingCallback = null,
        Action<SimpleShellTransitionArgs> pushingCallback = null,
        Action<SimpleShellTransitionArgs> poppingCallback = null,
        Func<SimpleShellTransitionArgs, uint> duration = null,
        Action<SimpleShellTransitionArgs> starting = null,
        Action<SimpleShellTransitionArgs> finished = null,
        Func<SimpleShellTransitionArgs, bool> destinationPageInFront = null,
        Func<SimpleShellTransitionArgs, Easing> easing = null)
    {
        page.SetTransition(new SimpleShellTransition(args =>
        {
            var action = args.TransitionType switch
            {
                SimpleShellTransitionType.Switching => switchingCallback,
                SimpleShellTransitionType.Pushing => pushingCallback,
                SimpleShellTransitionType.Popping => poppingCallback,
                _ => null
            };

            action?.Invoke(args);
        }, duration, starting, finished, destinationPageInFront, easing));
    }


    internal static IEnumerable<ShellSection> GetShellSections(this BaseShellItem baseShellItem)
    {
        var list = new HashSet<ShellSection>();

        if (baseShellItem is ShellSection shellSection)
        {
            list.Add(shellSection);
        }
        else if (baseShellItem is ShellItem shellItem)
        {
            foreach (var item in shellItem.Items)
            {
                var shellSections = GetShellSections(item);
                foreach (var section in shellSections)
                    list.Add(section);
            }
        }

        return list;
    }

    internal static IEnumerable<ShellContent> GetShellContents(this BaseShellItem baseShellItem)
    {
        var list = new HashSet<ShellContent>();

        if (baseShellItem is ShellContent shellContent)
        {
            list.Add(shellContent);
        }
        else if (baseShellItem is ShellItem shellItem)
        {
            foreach (var item in shellItem.Items)
            {
                var shellContents = GetShellContents(item);
                foreach (var content in shellContents)
                    list.Add(content);
            }
        }
        else if (baseShellItem is ShellSection shellSection)
        {
            foreach (var content in shellSection.Items)
                list.Add(content);
        }

        return list;
    }
}