#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

using SimpleToolkit.SimpleShell.Transitions;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public partial class NativeSimpleStackNavigationManager
{
    protected Task NavigateNativelyToPageInContainer(
        SimpleShell shell,
        IView previousShellItemContainer,
        IView previousShellSectionContainer,
        IView previousPage,
        bool isPreviousPageRoot,
        Func<IView, PlatformSimpleShellTransition> pageTransition,
        Func<SimpleShellTransitionArgs> args,
        bool animated = true)
    {
        throw new NotImplementedException();
    }

    protected void HandleNewStack(
        IReadOnlyList<IView> newPageStack,
        Func<IView, PlatformSimpleShellTransition> pageTransition,
        Func<SimpleShellTransitionArgs> args,
        bool animated = true)
    {
        NavigationStack = newPageStack;
        throw new NotImplementedException();
    }
}

#endif