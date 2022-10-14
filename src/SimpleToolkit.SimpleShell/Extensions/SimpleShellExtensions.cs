using SimpleToolkit.SimpleShell.Transitions;

namespace SimpleToolkit.SimpleShell.Extensions
{
    public static class SimpleShellExtensions
    {
        /// <summary>
        /// Sets the transition to the page.
        /// </summary>
        /// <param name="page">Destination page of a navigation with this transition.</param>
        /// <param name="transition">Transition of a navigation to the page.</param>
        public static void SetTransition(this Page page, SimpleShellTransition transition)
        {
            SimpleShell.SetTransition(page, transition);
        }

        /// <summary>
        /// Sets new transition to the page.
        /// </summary>
        /// <param name="page">Destination page of a navigation with this transition.</param>
        /// <param name="callback">Callback that is called when progress of the transition changes.</param>
        /// <param name="duration">Duration of the transition.</param>
        /// <param name="starting">Callback that is called when the transition starts.</param>
        /// <param name="finished">Callback that is called when the transition finishes.</param>
        /// <param name="destinationPageInFrontOnSwitching">Whether the destination page should be displayed in front of the origin page when switching root pages in the stack.</param>
        /// <param name="destinationPageInFrontOnPushing">Whether the destination page should be displayed in front of the origin page when pushing new page to the stack.</param>
        /// <param name="destinationPageInFrontOnPopping">Whether the destination page should be displayed in front of the origin page when popping existing page from the stack.</param>
        public static void SetTransition(
            this Page page,
            Action<SimpleShellTransitionArgs> callback,
            uint duration = SimpleShellTransition.DefaultDuration,
            Action<SimpleShellTransitionArgs> starting = null,
            Action<SimpleShellTransitionArgs> finished = null,
            bool destinationPageInFrontOnSwitching = SimpleShellTransition.DefaultDestinationPageInFrontOnSwitching,
            bool destinationPageInFrontOnPushing = SimpleShellTransition.DefaultDestinationPageInFrontOnPushing,
            bool destinationPageInFrontOnPopping = SimpleShellTransition.DefaultDestinationPageInFrontOnPopping)
        {
            page.SetTransition(new SimpleShellTransition(callback, duration, starting, finished, destinationPageInFrontOnSwitching, destinationPageInFrontOnPushing, destinationPageInFrontOnPopping));
        }

        /// <summary>
        /// Sets new transition to the page.
        /// </summary>
        /// <param name="page">Destination page of a navigation with this transition.</param>
        /// <param name="callback">Callback that is called when progress of the transition changes.</param>
        /// <param name="duration">Duration of the transition.</param>
        /// <param name="starting">Callback that is called when the transition starts.</param>
        /// <param name="finished">Callback that is called when the transition finishes.</param>
        /// <param name="destinationPageInFront">Whether the destination page should be displayed in front of the origin page when transitioning from one page to another.</param>
        public static void SetTransition(
            this Page page,
            Action<SimpleShellTransitionArgs> callback,
            Func<SimpleShellTransitionArgs, uint> duration = null,
            Action<SimpleShellTransitionArgs> starting = null,
            Action<SimpleShellTransitionArgs> finished = null,
            Func<SimpleShellTransitionArgs, bool> destinationPageInFront = null)
        {
            page.SetTransition(new SimpleShellTransition(callback, duration, starting, finished, destinationPageInFront));
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
}
