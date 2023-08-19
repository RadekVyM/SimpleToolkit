#if WINDOWS

using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
using NavFrame = Microsoft.UI.Xaml.Controls.Grid;
using Microsoft.UI.Xaml;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public abstract partial class BaseSimpleStackNavigationManager
{
    protected virtual void AddPlatformPageToContainer(IView newPage, SimpleShell shell, bool onTop = true, bool isCurrentPageRoot = true)
    {
        var newPageView = GetPlatformView(newPage);

        if (newPageView is null)
            return;

        if (isCurrentPageRoot)
            AddPlatformRootPage(onTop, newPageView);
        else
            AddViewToContainer(newPageView, navigationFrame, onTop);
    }

    protected virtual void RemovePlatformPageFromContainer(IView oldPage, IView oldShellSectionContainer, bool isCurrentPageRoot, bool isPreviousPageRoot)
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

    protected private void AddPlatformRootPage(bool onTop, PlatformView newPageView)
    {
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

        if (container is not null &&
            GetPageContainerNavHost(this.rootPageContainer) is NavFrame rootNavHost)
        {
            if (!navigationFrame.Children.Contains(container))
                AddViewToContainer(container, navigationFrame, onTop);

            if (sectionContainer is not null)
            {
                if (!rootNavHost.Children.Contains(sectionContainer))
                    AddViewToContainer(sectionContainer, rootNavHost, onTop);

                AddViewToContainer(newPageView, sectionNavHost, onTop);
            }
            else
            {
                AddViewToContainer(newPageView, rootNavHost, onTop);
            }
        }
        else
        {
            if (sectionContainer is not null)
            {
                if (!navigationFrame.Children.Contains(sectionContainer))
                    AddViewToContainer(sectionContainer, navigationFrame, onTop);

                AddViewToContainer(newPageView, sectionNavHost, onTop);
            }
            else
            {
                AddViewToContainer(newPageView, navigationFrame, onTop);
            }
        }
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

    protected private IList<UIElement> RemoveRootPageContainer(PlatformView oldContainer)
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

    protected private void RemoveShellSectionContainer(IView oldShellSectionContainer)
    {
        var oldContainer = GetPlatformView(oldShellSectionContainer);

        if (GetPageContainerNavHost(oldShellSectionContainer) is NavFrame oldNavHost)
            oldNavHost.Children.Clear();

        if (oldContainer.Parent is NavFrame parent)
            parent.Children.Remove(oldContainer);
    }

    protected private static void AddViewToContainer(PlatformView view, NavFrame container, bool onTop)
    {
        if (container.Children.Contains(view))
            container.Children.Remove(view);

        if (onTop)
            container.Children.Add(view);
        else
            container.Children.Insert(0, view);
    }
}

#endif