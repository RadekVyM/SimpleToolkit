#if IOS || MACCATALYST

using Microsoft.Maui.Platform;
using PlatformView = UIKit.UIView;
using NavFrame = UIKit.UIView;
using Microsoft.Maui.Handlers;
using SimpleToolkit.SimpleShell.Platform;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public abstract partial class BaseSimpleStackNavigationManager
{
    protected virtual void AddPlatformPageToContainer(IView newPage, SimpleShell shell, bool onTop = true, bool isCurrentPageRoot = true)
    {
        if (navigationFrame.NextResponder is not SimpleShellSectionContentController contentController)
            return;

        var newPageView = GetPlatformView(newPage);

        if (newPageView is null)
            return;

        newPageView.RemoveFromSuperview();

        contentController.DismissViewController(false, null);

        if (newPage.Handler is PageHandler pageHandler)
        {
            pageHandler.ViewController.RemoveFromParentViewController();
            contentController.AddChildViewController(pageHandler.ViewController);
        }

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

    protected virtual void RemovePlatformPageFromContainer(IView oldPage, IView oldShellSectionContainer, bool isCurrentPageRoot, bool isPreviousPageRoot)
    {
        var oldPageView = GetPlatformView(oldPage);

        if (oldPageView is null)
            return;

        if (oldPage.Handler is PageHandler pageHandler)
        {
            pageHandler.ViewController?.WillMoveToParentViewController(null);
            pageHandler.ViewController?.RemoveFromParentViewController();
        }

        oldPageView?.RemoveFromSuperview();

        if (oldShellSectionContainer is not null && currentShellSectionContainer != oldShellSectionContainer)
            RemoveShellSectionContainer(oldShellSectionContainer);

        if (!isCurrentPageRoot && isPreviousPageRoot && this.rootPageContainer is not null)
            RemoveRootPageContainer(this.rootPageContainer);
    }

    protected void AddPlatformRootPage(bool onTop, PlatformView newPageView)
    {
        var r = AddToContainer(this.rootPageContainer, navigationFrame, onTop);
        var s = AddToContainer(currentShellSectionContainer, r, onTop);
        AddToContainer(newPageView, s, onTop);
    }

    private NavFrame AddToContainer(IView childContainer, NavFrame parentNavHost, bool onTop)
    {
        _ = parentNavHost ?? throw new ArgumentNullException(nameof(parentNavHost), $"{nameof(SimpleNavigationHost)} is missing");

        if (childContainer is null)
            return parentNavHost;

        var platformChildContainer = GetPlatformView(childContainer);

        if (!parentNavHost.Subviews.Contains(platformChildContainer))
            parentNavHost.AddSubview(platformChildContainer);

        parentNavHost.BringSubviewToFront(onTop ? platformChildContainer : parentNavHost.Subviews.FirstOrDefault());

        var childContainerNavHost = GetPageContainerNavHost(childContainer);

        if (childContainerNavHost is NavFrame navHost)
            return navHost;

        return null;
    }

    private void AddToContainer(PlatformView child, NavFrame parentNavHost, bool onTop)
    {
        _ = parentNavHost ?? throw new ArgumentNullException(nameof(parentNavHost), $"{nameof(SimpleNavigationHost)} is missing");

        if (!parentNavHost.Subviews.Contains(child))
            parentNavHost.AddSubview(child);

        parentNavHost.BringSubviewToFront(onTop ? child : parentNavHost.Subviews.FirstOrDefault());
    }

    protected virtual void ReplaceRootPageContainer(IView rootPageContainer, bool isCurrentPageRoot)
    {
        var oldContainer = this.rootPageContainer;
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

            if (GetPageContainerNavHost(rootPageContainer) is PlatformView newNavHost)
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

    protected private IList<PlatformView> RemoveRootPageContainer(IView oldRootContainer)
    {
        var oldContainer = GetPlatformView(oldRootContainer);
        var oldChildren = new List<PlatformView>();

        if (GetPageContainerNavHost(oldRootContainer) is PlatformView oldNavHost)
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

        if (GetPageContainerNavHost(oldShellSectionContainer) is PlatformView oldNavHost)
            oldNavHost.ClearSubviews();

        oldContainer.RemoveFromSuperview();
    }
}

#endif