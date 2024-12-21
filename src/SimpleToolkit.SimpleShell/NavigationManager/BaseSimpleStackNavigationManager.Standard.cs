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


    protected private partial List<object> RemoveContainer(IView oldContainer, object? parent = null)
    {
        throw new NotSupportedException();
    }

    private static partial void AddChild(object parent, object child)
    {
    }

    private static partial void ClearChildren(object parent, List<object> oldChildren)
    {
    }
}

#endif