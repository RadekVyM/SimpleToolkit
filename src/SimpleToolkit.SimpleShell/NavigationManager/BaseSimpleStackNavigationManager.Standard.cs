#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

namespace SimpleToolkit.SimpleShell.NavigationManager;

public abstract partial class BaseSimpleStackNavigationManager
{
    protected virtual void AddPlatformPageToContainer(IView newPage, SimpleShell shell, bool onTop = true, bool isCurrentPageRoot = true)
    {
    }

    protected virtual void RemovePlatformPageFromContainer(
        IView oldPage,
        IView oldShellItemContainer,
        IView oldShellSectionContainer,
        bool isCurrentPageRoot,
        bool isPreviousPageRoot)
    {
    }


    protected private partial List<System.Object> RemoveContainer(IView oldContainer, System.Object parent = null)
    {
        throw new NotSupportedException();
    }

    private static partial void AddChild(System.Object parent, System.Object child)
    {
    }

    private static partial void ClearChildren(System.Object parent, List<System.Object> oldChildren)
    {
    }
}

#endif