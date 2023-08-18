#if ANDROID

using AndroidX.Fragment.App;
using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Platform;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public partial class NativeSimpleStackNavigationManager
{
    protected override void OnBackStackChanged(IReadOnlyList<IView> newPageStack)
    {
        HandleNewStack(newPageStack);
        FireNavigationFinished();

        return;
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
                shouldPop ? Resource.Animation.simpleshell_enter_left : Resource.Animation.simpleshell_enter_right,
                shouldPop ? Resource.Animation.simpleshell_exit_right : Resource.Animation.simpleshell_exit_left);
        }
        transaction.Replace(rootContainer.Id, fragment);
        transaction.Commit();
    }

    private Fragment CreateFragment(AView view)
    {
        if (view.Parent is Android.Views.ViewGroup vg)
            vg.RemoveView(view);

        return new SimpleFragment(view, mauiContext);
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