#if ANDROID

using Android.Views;
using PlatformView = Android.Views.View;
using NavFrame = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
using SimpleToolkit.SimpleShell.Extensions;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public abstract partial class BaseSimpleStackNavigationManager
{
    protected virtual void AddPlatformPageToContainer(IView newPage, SimpleShell shell, bool onTop = true, bool isCurrentPageRoot = true)
    {
        var newPageView = GetPlatformView(newPage);

        if (newPageView is null)
            return;

        if (newPageView.Parent is ViewGroup vg)
            vg.RemoveView(newPageView);

        if (isCurrentPageRoot)
        {
            AddPlatformRootPage(onTop, newPageView);
        }
        else
        {
            navigationFrame.AddView(newPageView);
            navigationFrame.BringChildToFront(onTop ? newPageView : navigationFrame.GetChildAt(0));
        }
    }

    protected virtual void RemovePlatformPageFromContainer(IView oldPage, IView oldShellSectionContainer, bool isCurrentPageRoot, bool isPreviousPageRoot)
    {
        var oldPageView = GetPlatformView(oldPage);

        if (oldPageView is null)
            return;

        if (oldPageView?.Parent is ViewGroup parent)
            parent.RemoveView(oldPageView);

        if (oldShellSectionContainer is not null && currentShellSectionContainer != oldShellSectionContainer)
            RemoveShellSectionContainer(oldShellSectionContainer);

        if (!isCurrentPageRoot && isPreviousPageRoot && this.rootPageContainer is not null)
            RemoveRootPageContainer(this.rootPageContainer);
    }

    protected private void AddPlatformRootPage(bool onTop, PlatformView newPageView)
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

        AddToContainer(platformChildContainer, parentNavHost, onTop);

        if (GetPageContainerNavHost(childContainer) is NavFrame navHost)
            return navHost;

        return null;
    }

    private void AddToContainer(PlatformView child, NavFrame parentNavHost, bool onTop)
    {
        _ = parentNavHost ?? throw new ArgumentNullException(nameof(parentNavHost), $"{nameof(SimpleNavigationHost)} is missing");

        if (!parentNavHost.Contains(child))
            parentNavHost.AddView(child);

        parentNavHost.BringChildToFront(onTop ? child : parentNavHost.GetChildAt(0));
    }

    protected virtual void ReplaceRootPageContainer(IView rootPageContainer, bool isCurrentPageRoot)
    {
        var oldContainer = this.rootPageContainer;
        var newContainer = GetPlatformView(rootPageContainer);
        IList<PlatformView> oldChildren = new List<PlatformView>();

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

            if (GetPageContainerNavHost(rootPageContainer) is NavFrame newNavHost)
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

    protected private IList<PlatformView> RemoveRootPageContainer(IView oldRootContainer)
    {
        var oldContainer = GetPlatformView(oldRootContainer);
        var oldChildren = new List<PlatformView>();

        if (GetPageContainerNavHost(oldRootContainer) is NavFrame oldNavHost)
        {
            oldChildren = oldNavHost.GetChildViews().ToList();
            oldNavHost.RemoveAllViews();
        }

        if (navigationFrame.Contains(oldContainer))
            navigationFrame.RemoveView(oldContainer);

        return oldChildren;
    }

    protected private void RemoveShellSectionContainer(IView oldShellSectionContainer)
    {
        var oldContainer = GetPlatformView(oldShellSectionContainer);

        if (GetPageContainerNavHost(oldShellSectionContainer) is NavFrame oldNavHost)
            oldNavHost.RemoveAllViews();

        if (oldContainer.Parent is ViewGroup parent)
            parent.RemoveView(oldContainer);
    }
}

#endif