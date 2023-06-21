#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

using PlatformView = System.Object;

namespace SimpleToolkit.SimpleShell.NavigationManager
{
    public partial class SimpleStackNavigationManager
    {
        protected virtual void AddPlatformPage(PlatformView newPageView, bool onTop = true, bool isCurrentPageRoot = true)
        {
        }

        protected virtual void RemovePlatformPage(PlatformView oldPageView, bool isCurrentPageRoot, bool isPreviousPageRoot)
        {
        }

        protected virtual void ReplaceRootPageContainer(IView rootPageContainer, bool isCurrentPageRoot)
        {
        }
    }
}

#endif