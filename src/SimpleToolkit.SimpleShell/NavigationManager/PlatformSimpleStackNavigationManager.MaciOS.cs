﻿#if IOS || MACCATALYST

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
        IView? previousShellItemContainer,
        IView? previousShellSectionContainer,
        IView? previousPage,
        bool isPreviousPageRoot,
        Func<IView, PlatformSimpleShellTransition?> pageTransition,
        Func<SimpleShellTransitionArgs> args,
        bool animated = true)
    {
        var transition = pageTransition(currentPage);
        var newPageView = GetPlatformView(currentPage) ?? throw new NullReferenceException("PlatformView cannot be null here.");
        var oldPageView = GetPlatformView(previousPage);
        var newSectionContainer = GetPlatformView(currentShellSectionContainer);
        var oldSectionContainer = GetPlatformView(previousShellSectionContainer);
        var newItemContainer = GetPlatformView(currentShellItemContainer);
        var oldItemContainer = GetPlatformView(previousShellItemContainer);

        var to = BaseSimpleStackNavigationManager.GetFirstDifferent(newItemContainer, newSectionContainer, newPageView, oldItemContainer, oldSectionContainer)!;
        var from = BaseSimpleStackNavigationManager.GetFirstDifferent(oldItemContainer, oldSectionContainer, oldPageView, newItemContainer, newSectionContainer);

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
        Func<IView, PlatformSimpleShellTransition?> pageTransition,
        Func<SimpleShellTransitionArgs>? args,
        bool animated = true)
    {
        _ = rootContainer ?? throw new NullReferenceException("rootContainer cannot be null here.");
        _ = navigationFrame ?? throw new NullReferenceException("navigationFrame cannot be null here.");

        var oldPageStack = NavigationStack;
        NavigationStack = newPageStack;
        var controller = rootContainer.NextResponder as PlatformSimpleShellSectionController ??
            throw new Exception("Unexpected type of UIViewController");
        var rootController = navigationFrame.NextResponder as UIViewController ??
            throw new Exception("Unexpected type of UIViewController");

        var newControllers = newPageStack
            .Skip(1)
            .Select(p =>
            {
                if (p.ToHandler(mauiContext) is not PageHandler pageHandler)
                    throw new InvalidOperationException("Handler of a page which you are navigating to is not inherited from PageHandler");

                return pageHandler.ViewController ?? throw new NullReferenceException("Page ViewController should not be null here.");
            })
            .Prepend(rootController)
            .ToArray();

        await controller.HandleNewStack(
            newControllers,
            GetTransitionPairs(newPageStack, newControllers, pageTransition, args),
            animated);
    }

    private void SwitchPlatformPages(
        UIView from,
        UIView to,
        IView? previousShellItemContainer,
        IView? previousShellSectionContainer,
        IView? previousPage,
        bool isPreviousPageRoot,
        Func<SimpleShellTransitionArgs> args,
        PlatformSimpleShellTransition? transition)
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
            if (transition?.SwitchingAnimationStarting is {} animation)
                animation(args())?.Invoke(from, to);

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

                    if (transition?.SwitchingAnimationFinished is {} animation)
                        animation(args())?.Invoke(from, to);
                });
            });
        }
    }

    private static IDictionary<UIViewController, PlatformSimpleShellControllerTransitionPair> GetTransitionPairs(
        IReadOnlyList<IView> newPageStack,
        UIViewController[] newControllers,
        Func<IView, PlatformSimpleShellTransition?> pageTransition,
        Func<SimpleShellTransitionArgs>? args)
    {
        var dictionary = new Dictionary<UIViewController, PlatformSimpleShellControllerTransitionPair>();
        var lastIndex = newControllers.Length - 1;
        var transition = pageTransition?.Invoke(newPageStack[lastIndex]);

        dictionary[newControllers[lastIndex]] = new PlatformSimpleShellControllerTransitionPair(
            GetValue(transition, args, transition?.PushingAnimation, null),
            GetValue(transition, args, transition?.PoppingAnimation, null));

        // TODO: Add a pair for interactive transition, if I find out how to implement it

        return dictionary;
    }
}

#endif