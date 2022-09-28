using SimpleToolkit.SimpleShell.Transitions;

namespace SimpleToolkit.SimpleShell.Extensions
{
    public static class SimpleShellExtensions
    {
        public static void SetTransition(this Page page, SimpleShellTransition transition)
        {
            SimpleShell.SetTransition(page, transition);
        }

        public static void SetTransition(
            this Page page,
            Action<SimpleShellTransitionArgs> callback,
            uint duration = SimpleShellTransition.DefaultDuration,
            Action<SimpleShellTransitionArgs> starting = null,
            Action<SimpleShellTransitionArgs> finished = null,
            bool destinationPageAboveOnSwitching = SimpleShellTransition.DefaultDestinationPageAboveOnSwitching,
            bool destinationPageAboveOnPushing = SimpleShellTransition.DefaultDestinationPageAboveOnPushing,
            bool destinationPageAboveOnPopping = SimpleShellTransition.DefaultDestinationPageAboveOnPopping)
        {
            page.SetTransition(new SimpleShellTransition(callback, duration, starting, finished, destinationPageAboveOnSwitching, destinationPageAboveOnPushing, destinationPageAboveOnPopping));
        }

        public static void SetTransition(
            this Page page,
            Action<SimpleShellTransitionArgs> callback,
            Func<SimpleShellTransitionArgs, uint> duration = null,
            Action<SimpleShellTransitionArgs> starting = null,
            Action<SimpleShellTransitionArgs> finished = null,
            Func<SimpleShellTransitionArgs, bool> destinationPageAboveOnSwitching = null,
            Func<SimpleShellTransitionArgs, bool> destinationPageAboveOnPushing = null,
            Func<SimpleShellTransitionArgs, bool> destinationPageAboveOnPopping = null)
        {
            page.SetTransition(new SimpleShellTransition(callback, duration, starting, finished, destinationPageAboveOnSwitching, destinationPageAboveOnPushing, destinationPageAboveOnPopping));
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
