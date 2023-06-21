#if ANDROID

using Android.Views;
using PlatformView = Android.Views.View;
using NavFrame = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
using SimpleToolkit.SimpleShell.Extensions;

namespace SimpleToolkit.SimpleShell.NavigationManager
{
    public partial class SimpleStackNavigationManager
    {
        protected virtual void AddPlatformPage(PlatformView newPageView, bool onTop = true, bool isCurrentPageRoot = true)
        {
            if (newPageView is null)
                return;

            var container = GetPlatformView(this.rootPageContainer);

            if (container?.Id == -1)
                container.Id = PlatformView.GenerateViewId();

            if (isCurrentPageRoot &&
                container is not null &&
                GetRooPageContainerNavHost(this.rootPageContainer) is NavFrame navHost)
            {
                if (navigationFrame.FindViewById(container.Id) is null)
                    navigationFrame.AddView(container);

                navHost.AddView(newPageView);

                if (onTop)
                {
                    navigationFrame.BringChildToFront(container);
                    navHost.BringChildToFront(newPageView);
                }
                else
                {
                    navigationFrame.BringChildToFront(navigationFrame.GetChildAt(0));
                    navHost.BringChildToFront(navHost.GetChildAt(0));
                }
            }
            else
            {
                navigationFrame.AddView(newPageView);

                if (onTop)
                    navigationFrame.BringChildToFront(newPageView);
                else
                    navigationFrame.BringChildToFront(navigationFrame.GetChildAt(0));
            }
        }

        protected virtual void RemovePlatformPage(PlatformView oldPageView, bool isCurrentPageRoot, bool isPreviousPageRoot)
        {
            var container = GetPlatformView(this.rootPageContainer);

            if (oldPageView is not null && navigationFrame.Contains(oldPageView))
                navigationFrame.RemoveView(oldPageView);
            if (GetRooPageContainerNavHost(this.rootPageContainer) is NavFrame navHost &&
                navHost.Contains(oldPageView))
                navHost.RemoveView(oldPageView);
            if (!isCurrentPageRoot && isPreviousPageRoot && container is not null)
                RemoveRootPageContainer(container);
        }

        protected virtual void ReplaceRootPageContainer(IView rootPageContainer, bool isCurrentPageRoot)
        {
            var oldContainer = GetPlatformView(this.rootPageContainer);
            var newContainer = GetPlatformView(rootPageContainer);
            IList<PlatformView> oldChildren = new List<PlatformView>();

            if (oldContainer?.Id == -1)
                oldContainer.Id = PlatformView.GenerateViewId();
            if (newContainer?.Id == -1)
                newContainer.Id = PlatformView.GenerateViewId();

            if (oldContainer is not null)
                oldChildren = RemoveRootPageContainer(oldContainer);

            // Old container is being replaced or added
            if (newContainer is not null && isCurrentPageRoot)
            {
                // New container is being added
                if (oldContainer is null && navigationFrame.ChildCount != 0)
                {
                    foreach (var child in navigationFrame.GetChildViews())
                        oldChildren.Add(child);
                    navigationFrame.RemoveAllViews();
                }

                navigationFrame.AddView(newContainer);

                if (GetRooPageContainerNavHost(rootPageContainer) is NavFrame newNavHost)
                {
                    foreach (var child in oldChildren)
                        newNavHost.AddView(child);
                }
            }

            // Old container is being removed
            if (oldContainer is not null && newContainer is null && isCurrentPageRoot)
            {
                foreach (var child in oldChildren)
                    navigationFrame.AddView(child);
            }
        }

        private IList<PlatformView> RemoveRootPageContainer(PlatformView oldContainer)
        {
            var oldChildren = new List<PlatformView>();

            if (GetRooPageContainerNavHost(this.rootPageContainer) is NavFrame oldNavHost)
            {
                oldChildren = oldNavHost.GetChildViews().ToList();
                oldNavHost.RemoveAllViews();
            }

            if (navigationFrame.Contains(oldContainer))
                navigationFrame.RemoveView(oldContainer);

            return oldChildren;
        }
    }
}

#endif