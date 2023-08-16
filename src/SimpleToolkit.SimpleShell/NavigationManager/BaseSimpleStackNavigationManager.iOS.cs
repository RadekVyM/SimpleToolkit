#if IOS || MACCATALYST

using Microsoft.Maui.Platform;
using PlatformView = UIKit.UIView;
using NavFrame = UIKit.UIView;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public abstract partial class BaseSimpleStackNavigationManager
{
    protected void AddPlatformRootPage(bool onTop, PlatformView newPageView)
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
            if (sectionContainer is not null)
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

                if (sectionContainer is not null)
                    sectionNavHost.BringSubviewToFront(newPageView);
            }
            else
            {
                navigationFrame.BringSubviewToFront(navigationFrame.Subviews.FirstOrDefault());

                if (sectionContainer is not null)
                    sectionNavHost.BringSubviewToFront(sectionNavHost.Subviews.FirstOrDefault());
            }
        }
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

    protected private IList<PlatformView> RemoveRootPageContainer(PlatformView oldContainer)
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

    protected private void RemoveShellSectionContainer(IView oldShellSectionContainer)
    {
        var oldContainer = GetPlatformView(oldShellSectionContainer);

        if (GetPageContainerNavHost(oldShellSectionContainer) is NavFrame oldNavHost)
            oldNavHost.ClearSubviews();

        oldContainer.RemoveFromSuperview();
    }
}

#endif