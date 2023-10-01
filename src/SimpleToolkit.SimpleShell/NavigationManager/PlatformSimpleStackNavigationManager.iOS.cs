#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Platform;
using SimpleToolkit.SimpleShell.Transitions;
using UIKit;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public partial class PlatformSimpleStackNavigationManager
{
    protected Task NavigateWithPlatformTransitionToPageInContainer(
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

        AddPlatformPageToContainer(currentPage, shell, GetValue(transition, args, transition?.DestinationPageInFrontOnSwitching, false), isCurrentPageRoot: isCurrentPageRoot);

        if (from is not null)
        {
            if (animated)
                SwitchPlatformPages(from, to, previousShellItemContainer, previousShellSectionContainer, previousPage, isPreviousPageRoot, args, transition);
            else if (previousPage != currentPage)
                RemovePlatformPageFromContainer(previousPage, previousShellItemContainer, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);
        }

        return Task.CompletedTask;
    }

    protected async void HandleNewStack(
        IReadOnlyList<IView> newPageStack,
        Func<IView, PlatformSimpleShellTransition> pageTransition,
        Func<SimpleShellTransitionArgs> args,
        bool animated = true)
    {
        var oldPageStack = NavigationStack;
        NavigationStack = newPageStack;
        var controller = rootContainer.NextResponder as PlatformSimpleShellSectionController;
        var root = navigationFrame.NextResponder as UIViewController;

        var newControllers = newPageStack
            .Skip(1)
            .Select(p =>
            {
                if (p.ToHandler(mauiContext) is not PageHandler pageHandler)
                    throw new InvalidOperationException("Handler of a page which you are navigating to is not inherited from PageHandler");

                return pageHandler.ViewController;
            })
            .Prepend(root)
            .ToArray();

        await controller.HandleNewStack(
            newControllers,
            GetTransitionPairs(newPageStack, pageTransition),
            animated);
        DisconnectHandlers(oldPageStack.Skip(1).Except(newPageStack));
    }

    private void SwitchPlatformPages(
        UIView from,
        UIView to,
        IView previousShellItemContainer,
        IView previousShellSectionContainer,
        IView previousPage,
        bool isPreviousPageRoot,
        Func<SimpleShellTransitionArgs> args,
        PlatformSimpleShellTransition transition)
    {
        if (transition is null || transition?.SwitchingAnimation is null)
        {
            // Default transition
            UIView.TransitionNotify(from, to, 0.2, UIViewAnimationOptions.TransitionCrossDissolve, (finished) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (previousPage != currentPage)
                        RemovePlatformPageFromContainer(previousPage, previousShellItemContainer, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);
                });
            });
        }
        else
        {
            transition?.SwitchingAnimationStarting(args())?.Invoke(from, to);

            UIView.AnimateNotify(GetValue(transition, args, transition?.SwitchingAnimationDuration, 0.2d), () =>
            {
                transition?.SwitchingAnimation(args())?.Invoke(from, to);
            },
            (finished) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (previousPage != currentPage)
                        RemovePlatformPageFromContainer(previousPage, previousShellItemContainer, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);

                    transition?.SwitchingAnimationFinished(args())?.Invoke(from, to);
                });
            });
        }
    }

    private static PlatformSimpleShellControllerTransitionPair[] GetTransitionPairs(
        IReadOnlyList<IView> newPageStack,
        Func<IView, PlatformSimpleShellTransition> pageTransition)
    {
        return newPageStack
            .Select(page =>
            {
                var transition = pageTransition?.Invoke(page);

                return new PlatformSimpleShellControllerTransitionPair(
                    GetValue(transition, transition?.PushingAnimation, null),
                    GetValue(transition, transition?.PoppingAnimation, null));
            })
            .ToArray();
    }

    protected static void DisconnectHandlers(IEnumerable<IView> pageStack)
    {
        foreach (var page in pageStack)
        {
            var handler = page.Handler;
            handler?.DisconnectHandler();
        }
    }
}

#endif