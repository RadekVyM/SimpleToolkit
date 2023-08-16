#if WINDOWS

using NavFrame = Microsoft.UI.Xaml.Controls.Grid;

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
                AddPlatformRootPage(onTop, newPageView);
            else
                AddViewToContainer(newPageView, navigationFrame, onTop);
        }

        protected virtual void RemovePlatformPage(IView oldPage, IView oldShellSectionContainer, bool isCurrentPageRoot, bool isPreviousPageRoot)
        {
            var oldPageView = GetPlatformView(oldPage);

            if (oldPageView is null)
                return;

            var container = GetPlatformView(this.rootPageContainer);

            if (oldPageView?.Parent is NavFrame parent)
                parent.Children.Remove(oldPageView);

            if (oldShellSectionContainer is not null && currentShellSectionContainer != oldShellSectionContainer)
                RemoveShellSectionContainer(oldShellSectionContainer);

            if (!isCurrentPageRoot && isPreviousPageRoot && container is not null)
                RemoveRootPageContainer(container);
        }
    }
}

#endif