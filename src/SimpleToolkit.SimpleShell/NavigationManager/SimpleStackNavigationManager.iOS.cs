#if IOS || MACCATALYST

using Microsoft.Maui.Platform;
using PlatformView = UIKit.UIView;
using NavFrame = UIKit.UIView;
using Microsoft.Maui.Handlers;
using SimpleToolkit.SimpleShell.Platform;

namespace SimpleToolkit.SimpleShell.NavigationManager
{
    public partial class SimpleStackNavigationManager
    {
        protected virtual void AddPlatformPage(IView newPage, SimpleShell shell, bool onTop = true, bool isCurrentPageRoot = true)
        {
            if (navigationFrame.NextResponder is not SimpleShellSectionContentController contentController)
                return;

            var newPageView = GetPlatformView(newPage);

            if (newPageView is null)
                return;

            contentController.DismissViewController(false, null);

            if (newPage.Handler is PageHandler pageHandler)
                contentController.AddChildViewController(pageHandler.ViewController);

            if (isCurrentPageRoot)
            {
                AddPlatformRootPage(onTop, newPageView);
            }
            else
            {
                navigationFrame.AddSubview(newPageView);
                navigationFrame.BringSubviewToFront(onTop ? newPageView : navigationFrame.Subviews.FirstOrDefault());
            }

            if (newPage.Handler is PageHandler didMovePageHandler)
                didMovePageHandler.ViewController.DidMoveToParentViewController(contentController);
        }

        protected virtual void RemovePlatformPage(IView oldPage, IView oldShellSectionContainer, bool isCurrentPageRoot, bool isPreviousPageRoot)
        {
            var oldPageView = GetPlatformView(oldPage);

            if (oldPageView is null)
                return;

            if (oldPage.Handler is PageHandler pageHandler)
            {
                pageHandler.ViewController?.WillMoveToParentViewController(null);
                pageHandler.ViewController?.RemoveFromParentViewController();
            }

            var container = GetPlatformView(this.rootPageContainer);

            oldPageView?.RemoveFromSuperview();

            if (oldShellSectionContainer is not null && currentShellSectionContainer != oldShellSectionContainer)
                RemoveShellSectionContainer(oldShellSectionContainer);

            if (!isCurrentPageRoot && isPreviousPageRoot && container is not null)
                RemoveRootPageContainer(container);
        }
    }
}

#endif