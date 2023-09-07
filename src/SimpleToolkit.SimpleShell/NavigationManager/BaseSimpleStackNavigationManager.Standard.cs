#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

namespace SimpleToolkit.SimpleShell.NavigationManager;

public abstract partial class BaseSimpleStackNavigationManager
{
    protected virtual void AddPlatformPageToContainer(IView newPage, SimpleShell shell, bool onTop = true, bool isCurrentPageRoot = true)
    {
    }

    protected virtual void RemovePlatformPageFromContainer(IView oldPage, IView oldShellItemContainer, IView oldShellSectionContainer, bool isCurrentPageRoot, bool isPreviousPageRoot)
    {
    }

    protected virtual void ReplaceRootPageContainer(IView rootPageContainer, bool isCurrentPageRoot)
    {
    }
}

#endif