#if ANDROID

using Android.Views.Animations;
using AndroidX.Fragment.App;
using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Platform;
using AView = Android.Views.View;

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

        to?.Animation?.Cancel();
        to?.ClearAnimation();
        from?.Animation?.Cancel();
        from?.ClearAnimation();

        AddPlatformPageToContainer(currentPage, shell, false, isCurrentPageRoot: isCurrentPageRoot);

        if (from is not null)
        {
            var enterAnimation = AnimationUtils.LoadAnimation(mauiContext.Context, Resource.Animation.simpleshell_fade_in);
            enterAnimation.AnimationEnd += OnEnterAnimationEnded;

            // IDK why the delay is needed to play the animation
            await Task.Delay(10).ConfigureAwait(true);
            to.StartAnimation(enterAnimation);
            RemovePlatformPageFromContainer(previousPage, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);
        }
        else
        {
            FireNavigationFinished();
        }

        void OnEnterAnimationEnded(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
        {
            e.Animation.AnimationEnd -= OnEnterAnimationEnded;
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
        var fragment = CreateFragment(platformView);

        var fragmentManager = mauiContext.Context.GetFragmentManager();
        var transaction = fragmentManager.BeginTransaction();

        transaction.SetReorderingAllowed(true);
        if (animated)
        {
            var shouldPop = ShouldPop(newPageStack, oldPageStack);

            transaction.SetCustomAnimations(
                shouldPop ? Resource.Animation.simpleshell_none : Resource.Animation.simpleshell_enter_right,
                shouldPop ? Resource.Animation.simpleshell_exit_right : Resource.Animation.simpleshell_none);
        }
        transaction.Replace(rootContainer.Id, fragment);
        transaction.Commit();
    }

    private static Fragment CreateFragment(AView view)
    {
        if (view.Parent is Android.Views.ViewGroup vg)
            vg.RemoveView(view);

        return new SimpleFragment(view);
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