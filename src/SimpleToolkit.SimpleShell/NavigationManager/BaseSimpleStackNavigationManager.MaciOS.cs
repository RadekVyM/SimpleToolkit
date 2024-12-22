using Microsoft.Maui.Platform;
using PlatformView = UIKit.UIView;
using PlatformContainer = UIKit.UIView;
using NavFrame = UIKit.UIView;
using Microsoft.Maui.Handlers;
using SimpleToolkit.SimpleShell.Platform;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public abstract partial class BaseSimpleStackNavigationManager
{
    protected virtual void AddPlatformPageToContainer(IView newPage, SimpleShell shell, bool onTop = true, bool isCurrentPageRoot = true)
    {
        _ = navigationFrame ?? throw new NullReferenceException("navigationFrame should not be null here.");
        
        if (navigationFrame.NextResponder is not SimpleShellSectionContentController contentController)
            return;

        var newPageView = GetPlatformView(newPage) ?? throw new NullReferenceException("PlatformView should not be null here.");

        newPageView.RemoveFromSuperview();

        contentController.DismissViewController(false, null);

        if (newPage.Handler is PageHandler pageHandler)
        {
            pageHandler.ViewController?.RemoveFromParentViewController();
            if (pageHandler.ViewController is {} pageViewController)
                contentController.AddChildViewController(pageViewController);
        }

        if (isCurrentPageRoot)
        {
            AddPlatformRootPage(onTop, newPageView);
        }
        else
        {
            navigationFrame.AddSubview(newPageView);
            if ((onTop ? newPageView : navigationFrame.Subviews.FirstOrDefault()) is {} view)
                navigationFrame.BringSubviewToFront(view);
        }

        if (newPage.Handler is PageHandler didMovePageHandler)
            didMovePageHandler.ViewController?.DidMoveToParentViewController(contentController);
    }

    protected virtual void RemovePlatformPageFromContainer(
        IView? oldPage,
        IView? oldShellItemContainer,
        IView? oldShellSectionContainer,
        bool isCurrentPageRoot,
        bool isPreviousPageRoot)
    {
        var oldPageView = GetPlatformView(oldPage);

        if (oldPageView is null)
            return;

        if (oldPage?.Handler is PageHandler pageHandler)
        {
            pageHandler.ViewController?.WillMoveToParentViewController(null);
            pageHandler.ViewController?.RemoveFromParentViewController();
        }

        oldPageView?.RemoveFromSuperview();

        if (oldShellSectionContainer is not null && (currentShellSectionContainer != oldShellSectionContainer || currentShellItemContainer != oldShellItemContainer))
            RemoveContainer(oldShellSectionContainer);

        if (oldShellItemContainer is not null && currentShellItemContainer != oldShellItemContainer)
            RemoveContainer(oldShellItemContainer);

        if (!isCurrentPageRoot && isPreviousPageRoot && this.rootPageContainer is not null)
            RemoveContainer(this.rootPageContainer);
    }

    protected void AddPlatformRootPage(bool onTop, PlatformView newPageView)
    {
        _ = navigationFrame ?? throw new NullReferenceException("navigationFrame should not be null here.");
        
        var r = AddToContainer(this.rootPageContainer, navigationFrame, onTop) ?? throw new NullReferenceException($"{nameof(SimpleNavigationHost)} is missing");
        var i = AddToContainer(currentShellItemContainer, r, onTop) ?? throw new NullReferenceException($"{nameof(SimpleNavigationHost)} is missing");
        var s = AddToContainer(currentShellSectionContainer, i, onTop) ?? throw new NullReferenceException($"{nameof(SimpleNavigationHost)} is missing");
        AddToContainer(newPageView, s, onTop);
    }

    private NavFrame? AddToContainer(IView? childContainer, NavFrame parentNavHost, bool onTop)
    {
        if (childContainer is null)
            return parentNavHost;

        var platformChildContainer = GetPlatformView(childContainer) ?? throw new NullReferenceException($"PlatformView should not be null here");
        
        AddToContainer(platformChildContainer, parentNavHost, onTop);

        if (GetPageContainerNavHost(childContainer) is NavFrame navHost)
            return navHost;

        return null;
    }

    private void AddToContainer(PlatformView child, NavFrame parentNavHost, bool onTop)
    {
        if (!parentNavHost.Subviews.Contains(child))
            parentNavHost.AddSubview(child);

        if ((onTop ? child : parentNavHost.Subviews.FirstOrDefault()) is {} view)
            parentNavHost.BringSubviewToFront(view);
    }

    private static partial void AddChild(PlatformContainer parent, PlatformView child)
    {
        parent.AddSubview(child);
    }

    private static partial void ClearChildren(PlatformContainer parent, List<PlatformView> oldChildren)
    {
        if (!parent.Subviews.Any())
            return;

        foreach (var child in parent.Subviews)
            oldChildren.Add(child);
        parent.ClearSubviews();
    }

    protected private partial List<PlatformView> RemoveContainer(IView oldContainer, PlatformContainer? parent)
    {
        var oldPlatformContainer = GetPlatformView(oldContainer);
        var oldChildren = new List<PlatformView>();

        if (GetPageContainerNavHost(oldContainer) is PlatformView oldNavHost)
        {
            oldChildren = oldNavHost.Subviews.ToList();
            oldNavHost.ClearSubviews();
        }

        oldPlatformContainer?.RemoveFromSuperview();

        return oldChildren;
    }
}