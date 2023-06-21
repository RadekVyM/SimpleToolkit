#if WINDOWS

using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
using NavFrame = Microsoft.UI.Xaml.Controls.Grid;
using Microsoft.UI.Xaml;

namespace SimpleToolkit.SimpleShell.NavigationManager
{
    public partial class SimpleStackNavigationManager
    {
        protected virtual void AddPlatformPage(PlatformView newPageView, bool onTop = true, bool isCurrentPageRoot = true)
        {
            var container = GetPlatformView(this.rootPageContainer);

            if (isCurrentPageRoot &&
                container is not null &&
                GetPageContainerNavHost(this.rootPageContainer) is NavFrame navHost)
            {
                if (!navigationFrame.Children.Contains(container))
                {
                    if (onTop)
                        navigationFrame.Children.Add(container);
                    else
                        navigationFrame.Children.Insert(0, container);
                }

                if (onTop)
                    navHost.Children.Add(newPageView);
                else
                    navHost.Children.Insert(0, newPageView);
            }
            else
            {
                if (onTop)
                    navigationFrame.Children.Add(newPageView);
                else
                    navigationFrame.Children.Insert(0, newPageView);
            }
        }

        protected virtual void RemovePlatformPage(PlatformView oldPageView, IView oldShellSectionContainer, bool isCurrentPageRoot, bool isPreviousPageRoot)
        {
            var container = GetPlatformView(this.rootPageContainer);

            if (oldPageView is not null && navigationFrame.Children.Contains(oldPageView))
                navigationFrame.Children.Remove(oldPageView);
            if (GetPageContainerNavHost(this.rootPageContainer) is NavFrame navHost &&
                navHost.Children.Contains(oldPageView))
                navHost.Children.Remove(oldPageView);
            if (!isCurrentPageRoot && isPreviousPageRoot && container is not null)
                RemoveRootPageContainer(container);
        }

        protected virtual void ReplaceRootPageContainer(IView rootPageContainer, bool isCurrentPageRoot)
        {
            var oldContainer = GetPlatformView(this.rootPageContainer);
            var newContainer = GetPlatformView(rootPageContainer);
            IList<UIElement> oldChildren = new List<UIElement>();

            if (oldContainer is not null)
                oldChildren = RemoveRootPageContainer(oldContainer);

            // Old container is being replaced or added
            if (newContainer is not null && isCurrentPageRoot)
            {
                // New container is being added
                if (oldContainer is null && navigationFrame.Children.Any())
                {
                    foreach (var child in navigationFrame.Children)
                        oldChildren.Add(child);
                    navigationFrame.Children.Clear();
                }

                navigationFrame.Children.Add(newContainer);

                if (GetPageContainerNavHost(rootPageContainer) is NavFrame newNavHost)
                {
                    foreach (var child in oldChildren)
                        newNavHost.Children.Add(child);
                }
            }
            
            // Old container is being removed
            if (oldContainer is not null && newContainer is null && isCurrentPageRoot)
            {
                foreach (var child in oldChildren)
                    navigationFrame.Children.Add(child);
            }
        }

        private IList<UIElement> RemoveRootPageContainer(PlatformView oldContainer)
        {
            var oldChildren = new List<UIElement>();
            
            if (GetPageContainerNavHost(this.rootPageContainer) is NavFrame oldNavHost)
            {
                oldChildren = oldNavHost.Children.ToList();
                oldNavHost.Children.Clear();
            }

            if (navigationFrame.Children.Contains(oldContainer))
                navigationFrame.Children.Remove(oldContainer);

            return oldChildren;
        }
    }
}

#endif