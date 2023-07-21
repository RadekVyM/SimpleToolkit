#if IOS || MACCATALYST

using Microsoft.Maui.Platform;
using PlatformView = UIKit.UIView;
using NavFrame = UIKit.UIView;
using Microsoft.Maui.Handlers;
using SimpleToolkit.SimpleShell.Handlers;
using GameController;
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

            var container = GetPlatformView(this.rootPageContainer);
            PlatformView sectionContainer = null;
            NavFrame sectionNavHost = null;

            if (
                currentShellSectionContainer is not null &&
                GetPageContainerNavHost(currentShellSectionContainer) is NavFrame snh)
            {
                sectionContainer = GetPlatformView(currentShellSectionContainer);
                sectionNavHost = snh;
            }

            if (isCurrentPageRoot &&
                container is not null &&
                GetPageContainerNavHost(this.rootPageContainer) is NavFrame rootNavHost)
            {
                if (!navigationFrame.Subviews.Contains(container))
                    navigationFrame.AddSubview(container);

                if (sectionContainer is not null)
                {
                    if (!rootNavHost.Subviews.Contains(sectionContainer))
                        rootNavHost.AddSubview(sectionContainer);

                    sectionNavHost.AddSubview(newPageView);
                }
                else
                {
                    rootNavHost.AddSubview(newPageView);
                }

                if (onTop)
                {
                    navigationFrame.BringSubviewToFront(container);

                    if (sectionContainer is not null)
                    {
                        rootNavHost.BringSubviewToFront(sectionContainer);
                        sectionNavHost.BringSubviewToFront(newPageView);
                    }
                    else
                    {
                        rootNavHost.BringSubviewToFront(newPageView);
                    }
                }
                else
                {
                    navigationFrame.BringSubviewToFront(navigationFrame.Subviews.FirstOrDefault());
                    rootNavHost.BringSubviewToFront(rootNavHost.Subviews.FirstOrDefault());

                    if (sectionContainer is not null)
                        sectionNavHost.BringSubviewToFront(sectionNavHost.Subviews.FirstOrDefault());
                }
            }
            else
            {
                if (isCurrentPageRoot && sectionContainer is not null)
                {
                    if (!navigationFrame.Subviews.Contains(sectionContainer))
                        navigationFrame.AddSubview(sectionContainer);

                    sectionNavHost.AddSubview(newPageView);
                }
                else
                {
                    navigationFrame.AddSubview(newPageView);
                }

                if (onTop)
                {
                    navigationFrame.BringSubviewToFront(newPageView);

                    if (isCurrentPageRoot && sectionContainer is not null)
                        sectionNavHost.BringSubviewToFront(newPageView);
                }
                else
                {
                    navigationFrame.BringSubviewToFront(navigationFrame.Subviews.FirstOrDefault());

                    if (isCurrentPageRoot && sectionContainer is not null)
                        sectionNavHost.BringSubviewToFront(sectionNavHost.Subviews.FirstOrDefault());
                }
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

        protected virtual void ReplaceRootPageContainer(IView rootPageContainer, bool isCurrentPageRoot)
        {
            var oldContainer = GetPlatformView(this.rootPageContainer);
            var newContainer = GetPlatformView(rootPageContainer);
            IList<PlatformView> oldChildren = new List<PlatformView>();

            if (oldContainer is not null)
                oldChildren = RemoveRootPageContainer(oldContainer);

            // Old container is being replaced or added
            if (newContainer is not null && isCurrentPageRoot)
            {
                // New container is being added
                if (oldContainer is null && navigationFrame.Subviews.Any())
                {
                    foreach (var child in navigationFrame.Subviews)
                        oldChildren.Add(child);
                    navigationFrame.ClearSubviews();
                }

                navigationFrame.AddSubview(newContainer);

                if (GetPageContainerNavHost(rootPageContainer) is NavFrame newNavHost)
                {
                    foreach (var child in oldChildren)
                        newNavHost.AddSubview(child);
                }
            }

            // Old container is being removed
            if (oldContainer is not null && newContainer is null && isCurrentPageRoot)
            {
                foreach (var child in oldChildren)
                    navigationFrame.AddSubview(child);
            }
        }

        private IList<PlatformView> RemoveRootPageContainer(PlatformView oldContainer)
        {
            var oldChildren = new List<PlatformView>();

            if (GetPageContainerNavHost(this.rootPageContainer) is NavFrame oldNavHost)
            {
                oldChildren = oldNavHost.Subviews.ToList();
                oldNavHost.ClearSubviews();
            }

            oldContainer.RemoveFromSuperview();

            return oldChildren;
        }

        private void RemoveShellSectionContainer(IView oldShellSectionContainer)
        {
            var oldContainer = GetPlatformView(oldShellSectionContainer);

            if (GetPageContainerNavHost(oldShellSectionContainer) is NavFrame oldNavHost)
                oldNavHost.ClearSubviews();

            oldContainer.RemoveFromSuperview();
        }
    }
}

#endif