#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

using PlatformView = System.Object;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public partial class SimpleStackNavigationManager
{
    protected virtual void AddPlatformPage(IView newPage, SimpleShell shell, bool onTop = true, bool isCurrentPageRoot = true)
    {
    }

    protected virtual void RemovePlatformPage(IView oldPage, IView oldShellSectionContainer, bool isCurrentPageRoot, bool isPreviousPageRoot)
    {
    }
}

#endif