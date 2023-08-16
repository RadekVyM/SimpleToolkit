#if ANDROID

using Android.Views;
using PlatformView = Android.Views.View;
using NavFrame = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
using SimpleToolkit.SimpleShell.Extensions;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public abstract partial class BaseSimpleStackNavigationManager
{
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

        if (container?.Id == -1)
            container.Id = PlatformView.GenerateViewId();

        if (container is not null &&
            GetPageContainerNavHost(this.rootPageContainer) is NavFrame rootNavHost)
        {
            if (navigationFrame.FindViewById(container.Id) is null)
                navigationFrame.AddView(container);

            if (sectionContainer is not null)
            {
                if (!rootNavHost.Contains(sectionContainer))
                    rootNavHost.AddView(sectionContainer);

                sectionNavHost.AddView(newPageView);
            }
            else
            {
                rootNavHost.AddView(newPageView);
            }

            if (onTop)
            {
                navigationFrame.BringChildToFront(container);

                if (sectionContainer is not null)
                {
                    rootNavHost.BringChildToFront(sectionContainer);
                    sectionNavHost.BringChildToFront(newPageView);
                }
                else
                {
                    rootNavHost.BringChildToFront(newPageView);
                }
            }
            else
            {
                navigationFrame.BringChildToFront(navigationFrame.GetChildAt(0));
                rootNavHost.BringChildToFront(rootNavHost.GetChildAt(0));

                if (sectionContainer is not null)
                    sectionNavHost.BringChildToFront(rootNavHost.GetChildAt(0));
            }
        }
        else
        {
            if (sectionContainer is not null)
            {
                if (!navigationFrame.Contains(sectionContainer))
                    navigationFrame.AddView(sectionContainer);

                sectionNavHost.AddView(newPageView);
            }
            else
            {
                navigationFrame.AddView(newPageView);
            }

            if (onTop)
            {
                navigationFrame.BringChildToFront(newPageView);

                if (sectionContainer is not null)
                    sectionNavHost.BringChildToFront(newPageView);
            }
            else
            {
                navigationFrame.BringChildToFront(navigationFrame.GetChildAt(0));

                if (sectionContainer is not null)
                    sectionNavHost.BringChildToFront(sectionNavHost.GetChildAt(0));
            }
        }
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

    protected private IList<PlatformView> RemoveRootPageContainer(PlatformView oldContainer)
    {
        var oldChildren = new List<PlatformView>();

        if (GetPageContainerNavHost(this.rootPageContainer) is NavFrame oldNavHost)
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