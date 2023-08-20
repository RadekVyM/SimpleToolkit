#if WINDOWS

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using ContentPresenter = Microsoft.UI.Xaml.Controls.ContentPresenter;
using HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment;
using VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public partial class NativeSimpleStackNavigationManager
{
    protected async void NavigateNativelyToPageInContainer(
        SimpleShell shell,
        IView previousShellSectionContainer,
        IView previousPage,
        bool isPreviousPageRoot)
    {
        var newPageView = GetPlatformView(currentPage);
        var oldPageView = GetPlatformView(previousPage);
        var newSectionContainer = GetPlatformView(currentShellSectionContainer);
        var oldSectionContainer = GetPlatformView(previousShellSectionContainer);

        var to = newSectionContainer == oldSectionContainer ?
            newPageView :
            newSectionContainer ?? newPageView;
        var from = newSectionContainer == oldSectionContainer ?
            oldPageView :
            oldSectionContainer ?? oldPageView;

        to.Transitions = new TransitionCollection { new EntranceThemeTransition() };

        // Here we go again 😶
        await Task.Delay(10);
        
        AddPlatformPageToContainer(currentPage, shell, true, isCurrentPageRoot: isCurrentPageRoot);

        if (from is not null)
        {
            RemovePlatformPageFromContainer(previousPage, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);
            FireNavigationFinished();
        }
        else
        {
            FireNavigationFinished();
        }
    }

    protected void HandleNewStack(IReadOnlyList<IView> newPageStack, bool animated = true)
    {
        var isRootNavigation = newPageStack.Count == 1 && NavigationStack.Count == 1;
        var switchFragments = (NavigationStack.Count == 0) ||
            (!isRootNavigation && newPageStack[newPageStack.Count - 1] != NavigationStack[NavigationStack.Count - 1]);
        var oldPageStack = NavigationStack;
        NavigationStack = newPageStack;

        if (!switchFragments)
            return;

        var platformView = isCurrentPageRoot ?
            navigationFrame :
            GetPlatformView(newPageStack[newPageStack.Count - 1]);

        var transition = animated ?
            new SlideNavigationTransitionInfo()
            {
                Effect = ShouldPop(newPageStack, oldPageStack) ? SlideNavigationTransitionEffect.FromLeft : SlideNavigationTransitionEffect.FromRight
            } :
            null;
        var destinationPageType = GetDestinationPageType();
        rootContainer.Navigate(destinationPageType, platformView, transition);
    }

    protected virtual Type GetDestinationPageType() =>
        typeof(Microsoft.UI.Xaml.Controls.Page);

    private void OnNavigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        if (e.Content is not FrameworkElement fe)
            return;

        if (e.Content is not Microsoft.UI.Xaml.Controls.Page page)
            return;

        ContentPresenter presenter;

        if (page.Content is null)
        {
            presenter = new ContentPresenter()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            page.Content = presenter;
        }
        else
        {
            presenter = page.Content as ContentPresenter;
        }

        // At this point if the Content isn't a ContentPresenter the user has replaced
        // the conent so we just let them take control
        if (presenter is null || currentPage is null)
            return;

        try
        {
            var platformView = e.Parameter as FrameworkElement;
            presenter.Content = platformView;
        }
        catch (Exception)
        {
            FireNavigationFinished();
            throw;
        }
    }

    private static bool ShouldPop(IReadOnlyList<IView> newPageStack, IReadOnlyList<IView> oldPageStack)
    {
        IView lastSame = null;

        for (int i = 0; i < newPageStack.Count; i++)
        {
            if (i < oldPageStack.Count && newPageStack[i] == oldPageStack[i])
                lastSame = newPageStack[i];
            else
                break;
        }

        return (lastSame is null && oldPageStack.Count > 0 && newPageStack[0] != oldPageStack[0])
            || lastSame == newPageStack[newPageStack.Count - 1];
    }
}

#endif