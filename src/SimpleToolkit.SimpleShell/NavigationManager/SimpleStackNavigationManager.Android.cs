#if ANDROID

using Android.Views;
using PlatformView = Android.Views.View;
using NavFrame = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
using SimpleToolkit.SimpleShell.Extensions;

namespace SimpleToolkit.SimpleShell.NavigationManager
{
    public partial class SimpleStackNavigationManager
    {
        protected virtual void AddPlatformPage(IView newPage, SimpleShell shell, bool onTop = true, bool isCurrentPageRoot = true)
        {
            var newPageView = GetPlatformView(newPage);

            if (newPageView is null)
                return;

            if (isCurrentPageRoot)
            {
                AddPlatformRootPage(onTop, newPageView);
            }
            else
            {
                navigationFrame.AddView(newPageView);
                navigationFrame.BringChildToFront(onTop ? newPageView : navigationFrame.GetChildAt(0));
            }
        }

        protected virtual void RemovePlatformPage(IView oldPage, IView oldShellSectionContainer, bool isCurrentPageRoot, bool isPreviousPageRoot)
        {
            var oldPageView = GetPlatformView(oldPage);

            if (oldPageView is null)
                return;

            var container = GetPlatformView(this.rootPageContainer);

            if (oldPageView?.Parent is ViewGroup parent)
                parent.RemoveView(oldPageView);

            if (oldShellSectionContainer is not null && currentShellSectionContainer != oldShellSectionContainer)
                RemoveShellSectionContainer(oldShellSectionContainer);

            if (!isCurrentPageRoot && isPreviousPageRoot && container is not null)
                RemoveRootPageContainer(container);
        }
    }
}

#endif