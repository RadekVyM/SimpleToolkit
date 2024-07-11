using Android.Views;
using PlatformView = Android.Views.View;
using PlatformContainer = Android.Views.ViewGroup;
using SimpleToolkit.SimpleShell.Extensions;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public abstract partial class BaseSimpleStackNavigationManager
{
    protected virtual void AddPlatformPageToContainer(IView newPage, SimpleShell shell, bool onTop = true, bool isCurrentPageRoot = true)
    {
        var newPageView = GetPlatformView(newPage);

        if (newPageView is null)
            return;

        if (newPageView.Parent is PlatformContainer vg)
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

    protected virtual void RemovePlatformPageFromContainer(
        IView oldPage,
        IView oldShellItemContainer,
        IView oldShellSectionContainer,
        bool isCurrentPageRoot,
        bool isPreviousPageRoot)
    {
        var oldPageView = GetPlatformView(oldPage);

        if (oldPageView is null)
            return;

        if (!isCurrentPageRoot && isPreviousPageRoot && this.rootPageContainer is not null)
            RemoveContainer(this.rootPageContainer, navigationFrame);

        if (oldShellItemContainer is not null && currentShellItemContainer != oldShellItemContainer)
            RemoveContainer(oldShellItemContainer);

        if (oldShellSectionContainer is not null && (currentShellSectionContainer != oldShellSectionContainer || currentShellItemContainer != oldShellItemContainer))
            RemoveContainer(oldShellSectionContainer);

        if (oldPageView?.Parent is PlatformContainer parent)
            parent.RemoveView(oldPageView);
    }

    protected private void AddPlatformRootPage(bool onTop, PlatformView newPageView)
    {
        var r = AddToContainer(this.rootPageContainer, navigationFrame, onTop);
        var i = AddToContainer(currentShellItemContainer, r, onTop);
        var s = AddToContainer(currentShellSectionContainer, i, onTop);
        AddToContainer(newPageView, s, onTop);
    }

    private PlatformContainer AddToContainer(IView childContainer, PlatformContainer parentNavHost, bool onTop)
    {
        _ = parentNavHost ?? throw new ArgumentNullException(nameof(parentNavHost), $"{nameof(SimpleNavigationHost)} is missing");

        if (childContainer is null)
            return parentNavHost;

        var platformChildContainer = GetPlatformView(childContainer);

        AddToContainer(platformChildContainer, parentNavHost, onTop);

        if (GetPageContainerNavHost(childContainer) is PlatformContainer navHost)
            return navHost;

        return null;
    }

    private void AddToContainer(PlatformView child, PlatformContainer parentNavHost, bool onTop)
    {
        _ = parentNavHost ?? throw new ArgumentNullException(nameof(parentNavHost), $"{nameof(SimpleNavigationHost)} is missing");

        if (!parentNavHost.Contains(child))
            parentNavHost.AddView(child);

        parentNavHost.BringChildToFront(onTop ? child : parentNavHost.GetChildAt(0));
    }

    private static partial void AddChild(PlatformContainer parent, PlatformView child)
    {
        parent.AddView(child);
    }

    private static partial void ClearChildren(PlatformContainer parent, List<PlatformView> oldChildren)
    {
        if (parent.ChildCount == 0)
            return;

        foreach (var child in parent.GetChildViews())
            oldChildren.Add(child);
        parent.RemoveAllViews();
    }

    protected private partial List<PlatformView> RemoveContainer(IView oldContainer, PlatformContainer parent = null)
    {
        var oldPlatformContainer = GetPlatformView(oldContainer);
        var oldChildren = new List<PlatformView>();

        if (GetPageContainerNavHost(oldContainer) is PlatformContainer oldNavHost)
        {
            oldChildren = oldNavHost.GetChildViews().ToList();
            oldNavHost.RemoveAllViews();
        }

        if (oldPlatformContainer.Parent is ViewGroup realParent)
            realParent.RemoveView(oldPlatformContainer);

        // This is here just to make sure that a root page container is really removed
        // However, it is probably not needed
        if (parent is not null && parent.Contains(oldPlatformContainer))
            parent.RemoveView(oldPlatformContainer);
        
        return oldChildren;
    }
}