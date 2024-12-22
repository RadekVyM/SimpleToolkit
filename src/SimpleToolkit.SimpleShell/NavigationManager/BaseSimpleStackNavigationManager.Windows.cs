using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
using PlatformPanel = Microsoft.UI.Xaml.Controls.Panel;
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
            AddToContainer(newPageView, navigationFrame, onTop);
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

        if (oldPageView?.Parent is PlatformPanel parent)
        {
            parent.Children.Remove(oldPageView);
        }
        else
        {
            if (GetPageContainerNavHost(oldShellSectionContainer) is PlatformPanel sectionNavHost)
                sectionNavHost.Children.Remove(oldPageView);
            if (GetPageContainerNavHost(oldShellItemContainer) is PlatformPanel itemNavHost)
                itemNavHost.Children.Remove(oldPageView);
            if (GetPageContainerNavHost(this.rootPageContainer) is PlatformPanel rootNavHost)
                rootNavHost.Children.Remove(oldPageView);
            navigationFrame.Children.Remove(oldPageView);
        }

        if (oldShellSectionContainer is not null && (currentShellSectionContainer != oldShellSectionContainer || currentShellItemContainer != oldShellItemContainer))
            RemoveContainer(oldShellSectionContainer);

        if (oldShellItemContainer is not null && currentShellItemContainer != oldShellItemContainer)
            RemoveContainer(oldShellItemContainer);

        if (!isCurrentPageRoot && isPreviousPageRoot && this.rootPageContainer is not null)
            RemoveContainer(this.rootPageContainer, navigationFrame);
    }

    protected private void AddPlatformRootPage(bool onTop, PlatformView newPageView)
    {
        var r = AddToContainer(this.rootPageContainer, navigationFrame, onTop);
        var i = AddToContainer(currentShellItemContainer, r, onTop);
        var s = AddToContainer(currentShellSectionContainer, i, onTop);
        AddToContainer(newPageView, s, onTop);
    }

    private PlatformPanel AddToContainer(IView childContainer, PlatformPanel parentNavHost, bool onTop)
    {
        _ = parentNavHost ?? throw new ArgumentNullException(nameof(parentNavHost), $"{nameof(SimpleNavigationHost)} is missing");

        if (childContainer is null)
            return parentNavHost;

        var platformChildContainer = GetPlatformView(childContainer);

        AddToContainer(platformChildContainer, parentNavHost, onTop);

        if (GetPageContainerNavHost(childContainer) is PlatformPanel navHost)
            return navHost;

        return null;
    }

    private void AddToContainer(PlatformView child, PlatformPanel parentNavHost, bool onTop)
    {
        _ = parentNavHost ?? throw new ArgumentNullException(nameof(parentNavHost), $"{nameof(SimpleNavigationHost)} is missing");

        if (parentNavHost.Children.Contains(child))
            parentNavHost.Children.Remove(child);

        if (onTop)
            parentNavHost.Children.Add(child);
        else
            parentNavHost.Children.Insert(0, child);
    }

    private static partial void AddChild(PlatformPanel parent, UIElement child)
    {
        parent.Children.Add(child);
    }

    private static partial void ClearChildren(PlatformPanel parent, List<UIElement> oldChildren)
    {
        if (!parent.Children.Any())
            return;

        foreach (var child in parent.Children)
            oldChildren.Add(child);
        parent.Children.Clear();
    }

    protected private partial List<UIElement> RemoveContainer(IView oldContainer, PlatformPanel? parent)
    {
        var oldPlatformContainer = GetPlatformView(oldContainer);
        var oldChildren = new List<UIElement>();

        if (GetPageContainerNavHost(oldContainer) is PlatformPanel oldNavHost)
        {
            oldChildren = oldNavHost.Children.ToList();
            oldNavHost.Children.Clear();
        }

        if (oldPlatformContainer.Parent is PlatformPanel realParent)
            realParent.Children.Remove(oldPlatformContainer);

        // This is here just to make sure that a root page container is really removed
        // However, it is probably not needed
        if (parent is not null && parent.Children.Contains(oldPlatformContainer))
            parent.Children.Remove(oldPlatformContainer);

        return oldChildren;
    }
}