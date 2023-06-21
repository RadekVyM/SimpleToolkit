#if IOS || MACCATALYST

using Microsoft.Maui.Platform;
using PlatformView = UIKit.UIView;
using NavFrame = UIKit.UIView;

namespace SimpleToolkit.SimpleShell.NavigationManager
{
    public partial class SimpleStackNavigationManager
    {
        protected virtual void AddPlatformPage(PlatformView newPageView, bool onTop = true, bool isCurrentPageRoot = true)
        {
            if (newPageView is null)
                return;

            var container = GetPlatformView(this.rootPageContainer);

            if (isCurrentPageRoot &&
                container is not null &&
                GetRooPageContainerNavHost(this.rootPageContainer) is NavFrame navHost)
            {
                if (!navigationFrame.Subviews.Contains(container))
                    navigationFrame.AddSubview(container);

                navHost.AddSubview(newPageView);

                if (onTop)
                {
                    navigationFrame.BringSubviewToFront(container);
                    navHost.BringSubviewToFront(newPageView);
                }
                else
                {
                    navigationFrame.BringSubviewToFront(navigationFrame.Subviews.FirstOrDefault());
                    navHost.BringSubviewToFront(navHost.Subviews.FirstOrDefault());
                }
            }
            else
            {
                navigationFrame.AddSubview(newPageView);

                if (onTop)
                    navigationFrame.BringSubviewToFront(newPageView);
                else
                    navigationFrame.BringSubviewToFront(navigationFrame.Subviews.FirstOrDefault());
            }
        }

        protected virtual void RemovePlatformPage(PlatformView oldPageView, bool isCurrentPageRoot, bool isPreviousPageRoot)
        {
            var container = GetPlatformView(this.rootPageContainer);

            oldPageView?.RemoveFromSuperview();
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

                if (GetRooPageContainerNavHost(rootPageContainer) is NavFrame newNavHost)
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

            if (GetRooPageContainerNavHost(this.rootPageContainer) is NavFrame oldNavHost)
            {
                oldChildren = oldNavHost.Subviews.ToList();
                oldNavHost.ClearSubviews();
            }

            oldContainer.RemoveFromSuperview();

            return oldChildren;
        }
    }
}

#endif