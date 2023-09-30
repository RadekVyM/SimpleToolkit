#if WINDOWS

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using ContentPresenter = Microsoft.UI.Xaml.Controls.ContentPresenter;
using HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment;
using VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment;
using SimpleToolkit.SimpleShell.Transitions;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public partial class NativeSimpleStackNavigationManager
{
    protected async Task NavigateNativelyToPageInContainer(
        SimpleShell shell,
        IView previousShellItemContainer,
        IView previousShellSectionContainer,
        IView previousPage,
        bool isPreviousPageRoot,
        Func<IView, PlatformSimpleShellTransition> pageTransition,
        Func<SimpleShellTransitionArgs> args,
        bool animated = true)
    {
        var transition = pageTransition(currentPage);
        var newPageView = GetPlatformView(currentPage);
        var oldPageView = GetPlatformView(previousPage);
        var newSectionContainer = GetPlatformView(currentShellSectionContainer);
        var oldSectionContainer = GetPlatformView(previousShellSectionContainer);
        var newItemContainer = GetPlatformView(currentShellItemContainer);
        var oldItemContainer = GetPlatformView(previousShellItemContainer);

        var to = GetFirstDifferent(newItemContainer, newSectionContainer, newPageView, oldItemContainer, oldSectionContainer);
        var from = GetFirstDifferent(oldItemContainer, oldSectionContainer, oldPageView, newItemContainer, newSectionContainer);

        ClearTransitions(newPageView, newSectionContainer, newItemContainer);
        
        var switchingTransition = GetValue(transition, args, transition?.SwitchingAnimation, new EntranceThemeTransition());

        if (animated && switchingTransition is not null)
            to.Transitions = new TransitionCollection { switchingTransition };
        else
            to.Transitions = new TransitionCollection { };

        // Here we go again 😶
        // The delay is needed to play the animation, but ideally it should not be
        await Task.Delay(10);

        AddPlatformPageToContainer(currentPage, shell, true, isCurrentPageRoot: isCurrentPageRoot);

        if (from is not null && previousPage != currentPage)
            RemovePlatformPageFromContainer(previousPage, previousShellItemContainer, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);
    }

    protected void HandleNewStack(
        IReadOnlyList<IView> newPageStack,
        Func<IView, PlatformSimpleShellTransition> pageTransition,
        Func<SimpleShellTransitionArgs> args,
        bool animated = true)
    {
        var transition = pageTransition(currentPage);
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

        var shouldPop = ShouldPop(newPageStack, oldPageStack);
        var defaultTransitionInfo = new SlideNavigationTransitionInfo()
        {
            Effect = SlideNavigationTransitionEffect.FromRight
        };

        var transitionInfo = animated ?
            (shouldPop ?
                GetValue(transition, args, transition?.PoppingAnimation, defaultTransitionInfo) :
                GetValue(transition, args, transition?.PushingAnimation, defaultTransitionInfo)) :
            null;
        var destinationPageType = GetDestinationPageType();

        if (shouldPop)
        {
            rootContainer.BackStack.Insert(0, new PageStackEntry(destinationPageType, platformView, transitionInfo));
            rootContainer.GoBack(transitionInfo);
        }
        else
        {
            rootContainer.Navigate(destinationPageType, platformView, transitionInfo);
        }

        rootContainer.BackStack.Clear();
        rootContainer.ForwardStack.Clear();
    }

    protected virtual Type GetDestinationPageType() =>
        typeof(Microsoft.UI.Xaml.Controls.Page);

    // Based on https://github.com/dotnet/maui/blob/main/src/Core/src/Platform/Windows/StackNavigationManager.cs
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

    private static void ClearTransitions(FrameworkElement newPageView, FrameworkElement newSectionContainer, FrameworkElement newItemContainer)
    {
        if (newPageView is not null)
            newPageView.Transitions = new TransitionCollection { };
        if (newSectionContainer is not null)
            newSectionContainer.Transitions = new TransitionCollection { };
        if (newItemContainer is not null)
            newItemContainer.Transitions = new TransitionCollection { };
    }
}

#endif