#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

using PlatformPage = System.Object;

namespace SimpleToolkit.SimpleShell.NavigationManager
{
    public partial class SimpleStackNavigationManager
    {
        protected virtual void AddPlatformPage(PlatformPage newPageView, bool onTop = true)
        {
        }

        protected virtual void RemovePlatformPage(PlatformPage oldPageView)
        {
        }

        protected virtual void ReplaceRootPageOverlay(IView rootPageOverlay)
        {
        }
    }
}

#endif