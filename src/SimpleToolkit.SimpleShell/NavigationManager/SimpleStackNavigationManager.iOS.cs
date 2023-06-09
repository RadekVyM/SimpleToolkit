#if IOS || MACCATALYST

using Microsoft.Maui.Platform;
using PlatformPage = UIKit.UIView;

namespace SimpleToolkit.SimpleShell.NavigationManager
{
    public partial class SimpleStackNavigationManager
    {
        protected virtual void AddPlatformPage(PlatformPage newPageView, bool onTop = true)
        {
            var overlay = GetPlatformView(this.rootPageContainer);

            navigationFrame.AddSubview(newPageView);

            if (isCurrentPageRoot && overlay is not null && !navigationFrame.Subviews.Contains(overlay))
                navigationFrame.AddSubview(overlay);

            if (onTop)
            {
                if (isCurrentPageRoot && overlay is not null)
                    navigationFrame.BringSubviewToFront(overlay);
                else
                    navigationFrame.BringSubviewToFront(newPageView);
            }
            else
            {
                if (isPreviousPageRoot && overlay is not null)
                    navigationFrame.BringSubviewToFront(overlay);

                navigationFrame.BringSubviewToFront(navigationFrame.Subviews.FirstOrDefault());
            }
        }
        
        protected virtual void RemovePlatformPage(PlatformPage oldPageView)
        {
            var overlay = GetPlatformView(this.rootPageContainer);

            oldPageView?.RemoveFromSuperview();
            if (!isCurrentPageRoot && isPreviousPageRoot && overlay is not null)
                overlay?.RemoveFromSuperview();
        }

        protected virtual void ReplaceRootPageContainer(IView rootPageContainer)
        {
            var oldOverlay = GetPlatformView(this.rootPageContainer);
            var newOverlay = GetPlatformView(rootPageContainer);

            oldOverlay?.RemoveFromSuperview();
            if (newOverlay is not null && isCurrentPageRoot)
                navigationFrame.AddSubview(newOverlay);
        }

    }
}

#endif