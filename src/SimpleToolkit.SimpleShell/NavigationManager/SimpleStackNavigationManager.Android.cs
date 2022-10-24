#if ANDROID

using Microsoft.Maui.Platform;
using PlatformPage = Android.Views.View;

namespace SimpleToolkit.SimpleShell.NavigationManager
{
    public partial class SimpleStackNavigationManager
    {
        protected virtual void AddPlatformPage(PlatformPage newPageView, bool onTop = true)
        {
            var overlay = GetPlatformView(this.rootPageOverlay);

            if (overlay?.Id == -1)
                overlay.Id = PlatformPage.GenerateViewId();

            navigationFrame.AddView(newPageView);

            if (isCurrentPageRoot && overlay is not null && navigationFrame.FindViewById(overlay.Id) is null)
                navigationFrame.AddView(overlay);

            if (onTop)
            {
                if (isCurrentPageRoot && overlay is not null)
                    navigationFrame.BringChildToFront(overlay);
                else
                    navigationFrame.BringChildToFront(newPageView);
            }
            else
            {
                if (isPreviousPageRoot && overlay is not null)
                    navigationFrame.BringChildToFront(overlay);

                navigationFrame.BringChildToFront(navigationFrame.GetChildAt(0));
            }
        }

        protected virtual void RemovePlatformPage(PlatformPage oldPageView)
        {
            var overlay = GetPlatformView(this.rootPageOverlay);

            if (oldPageView is not null)
                navigationFrame.RemoveView(oldPageView);
            if (!isCurrentPageRoot && isPreviousPageRoot && overlay is not null)
                navigationFrame.RemoveView(overlay);
        }

        protected virtual void ReplaceRootPageOverlay(IView rootPageOverlay)
        {
            var oldOverlay = GetPlatformView(this.rootPageOverlay);
            var newOverlay = GetPlatformView(rootPageOverlay);

            if (oldOverlay?.Id == -1)
                oldOverlay.Id = PlatformPage.GenerateViewId();
            if (newOverlay?.Id == -1)
                newOverlay.Id = PlatformPage.GenerateViewId();

            if (oldOverlay is not null)
                navigationFrame.RemoveView(oldOverlay);
            if (newOverlay is not null && isCurrentPageRoot && navigationFrame.FindViewById(newOverlay.Id) is null)
                navigationFrame.AddView(newOverlay);
        }
    }
}

#endif