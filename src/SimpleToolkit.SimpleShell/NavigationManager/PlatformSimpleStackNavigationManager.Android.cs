﻿using Android.Views.Animations;
using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Platform;
using SimpleToolkit.SimpleShell.Transitions;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public partial class PlatformSimpleStackNavigationManager
{
    private SimpleFragment? currentFragment;

    protected async Task NavigateWithPlatformTransitionToPageInContainer(
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

        var to = GetFirstDifferent(newItemContainer, newSectionContainer, newPageView, oldItemContainer, oldSectionContainer)!;
        var from = GetFirstDifferent(oldItemContainer, oldSectionContainer, oldPageView, newItemContainer, newSectionContainer);

        ClearAnimations(oldPageView, oldSectionContainer, oldItemContainer, to, from);

        AddPlatformPageToContainer(currentPage, shell, GetValue(transition, args, transition?.DestinationPageInFrontOnSwitching, false), isCurrentPageRoot: isCurrentPageRoot);

        if (from is not null && animated)
        {
            var context = mauiContext.Context;
            var enterAnimation = AnimationUtils.LoadAnimation(context, GetValue(transition, args, transition?.SwitchingEnterAnimation, Resource.Animation.simpleshell_fade_in))
                ?? throw new NullReferenceException("Enter animation could not be loaded.");
            var leaveAnimation = AnimationUtils.LoadAnimation(context, GetValue(transition, args, transition?.SwitchingLeaveAnimation, Resource.Animation.simpleshell_fade_out))
                ?? throw new NullReferenceException("Leave animation could not be loaded.");
            var noneAnimation = AnimationUtils.LoadAnimation(context, Resource.Animation.simpleshell_none)
                ?? throw new NullReferenceException("Animation could not be loaded.");

            noneAnimation.Duration = leaveAnimation.Duration;

            to.Visibility = Android.Views.ViewStates.Invisible;

            // The delay is needed to play the animation, but ideally it should not be
            await Task.Delay(10).ConfigureAwait(true);

            to.StartAnimation(enterAnimation);
            from.StartAnimation(leaveAnimation);
            to.Visibility = Android.Views.ViewStates.Visible;
            from.Visibility = Android.Views.ViewStates.Visible;

            // Make sure that all the disappearing containers stay visible
            if (from != oldPageView)
                oldPageView?.StartAnimation(noneAnimation);
            if (from != oldSectionContainer)
                oldSectionContainer?.StartAnimation(noneAnimation);
            if (from != oldItemContainer)
                oldItemContainer?.StartAnimation(noneAnimation);
        }

        // Animation is played even if the view is removed from its container in the meantime
        // The view is moved to a disappearing children collection of the container
        if (previousPage is not null && previousPage != currentPage)
            RemovePlatformPageFromContainer(previousPage, previousShellItemContainer, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);
    }

    protected void HandleNewStack(
        IReadOnlyList<IView> newPageStack,
        Func<IView, PlatformSimpleShellTransition?> pageTransition,
        Func<SimpleShellTransitionArgs>? args,
        bool animated = true)
    {
        var transition = pageTransition(currentPage);
        var isRootNavigation = newPageStack.Count == 1 && NavigationStack.Count == 1;
        var switchFragments = (NavigationStack.Count == 0) ||
            (!isRootNavigation && newPageStack[newPageStack.Count - 1] != NavigationStack[NavigationStack.Count - 1]);
        var oldPageStack = NavigationStack;
        NavigationStack = newPageStack;

        if (!switchFragments || navigationFrame is null || rootContainer is null)
            return;

        var previousFragment = currentFragment;
        var shouldPop = ShouldPop(newPageStack, oldPageStack);
        var destinationPageInFront =
            shouldPop ?
                GetValue(transition, args, transition?.DestinationPageInFrontOnPopping, false) :
                GetValue(transition, args, transition?.DestinationPageInFrontOnPushing, true);

        var platformView = isCurrentPageRoot ?
            navigationFrame :
            GetPlatformView(newPageStack[newPageStack.Count - 1]) ?? throw new NullReferenceException("PlatformView should not be null here.");
        currentFragment = CreateFragment(platformView);

        previousFragment?.ClearAnimationFinished();
        currentFragment.AnimationFinished += FragmentAnimationFinished;

        if (previousFragment is not null)
            previousFragment.OnTop = !destinationPageInFront;
        currentFragment.OnTop = destinationPageInFront;

        var fragmentManager = mauiContext.Context?.GetFragmentManager() ?? throw new NullReferenceException("FragmentManager should not be null here.");
        var transaction = fragmentManager.BeginTransaction() ;

        transaction.SetReorderingAllowed(true);

        if (animated)
        {
            transaction.SetCustomAnimations(
                shouldPop ?
                    GetValue(transition, args, transition?.PoppingEnterAnimation, Resource.Animation.simpleshell_none) :
                    GetValue(transition, args, transition?.PushingEnterAnimation, Resource.Animation.simpleshell_enter_right),
                shouldPop ?
                    GetValue(transition, args, transition?.PoppingLeaveAnimation, Resource.Animation.simpleshell_exit_right) :
                    GetValue(transition, args, transition?.PushingLeaveAnimation, Resource.Animation.simpleshell_none));
        }
        
        transaction.Replace(rootContainer.Id, currentFragment);
        transaction.Commit();

        static void FragmentAnimationFinished(object? sender, EventArgs args)
        {
            if (sender is SimpleFragment fragment)
                fragment.ClearAnimationFinished();
        }
    }

    private static SimpleFragment CreateFragment(AView view)
    {
        if (view.Parent is Android.Views.ViewGroup vg)
            vg.RemoveView(view);

        return new SimpleFragment(view);
    }

    private static bool ShouldPop(IReadOnlyList<IView> newPageStack, IReadOnlyList<IView> oldPageStack)
    {
        IView? lastSame = null;

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

    private static void ClearAnimations(AView? oldPageView, AView? oldSectionContainer, AView? oldItemContainer, AView? to, AView? from)
    {
        to?.Animation?.Cancel();
        to?.ClearAnimation();
        from?.Animation?.Cancel();
        from?.ClearAnimation();

        if (oldPageView?.Parent is Android.Views.ViewGroup pageVg)
            pageVg.ClearDisappearingChildren();
        if (oldSectionContainer?.Parent is Android.Views.ViewGroup sectionVg)
            sectionVg.ClearDisappearingChildren();
        if (oldItemContainer?.Parent is Android.Views.ViewGroup itemVg)
            itemVg.ClearDisappearingChildren();
    }
}