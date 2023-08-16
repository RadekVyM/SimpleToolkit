using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Extensions;
using SimpleToolkit.SimpleShell.Transitions;
#if ANDROID
using NavFrame = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
using PlatformView = Android.Views.View;
#elif __IOS__ || MACCATALYST
using UIKit;
using NavFrame = UIKit.UIView;
using PlatformView = UIKit.UIView;
#elif WINDOWS
using NavFrame = Microsoft.UI.Xaml.Controls.Grid;
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using NavFrame = System.Object;
using PlatformView = System.Object;
#endif

namespace SimpleToolkit.SimpleShell.NavigationManager;

public abstract partial class BaseSimpleStackNavigationManager : ISimpleStackNavigationManager
{
    protected IMauiContext mauiContext;
    protected NavFrame navigationFrame;
    protected IView currentPage;
    protected IView rootPageContainer;
    protected IView currentShellSectionContainer;
    protected bool isCurrentPageRoot = true;

    public IStackNavigation StackNavigation { get; protected set; }
    public IReadOnlyList<IView> NavigationStack { get; protected set; } = new List<IView>();


    public BaseSimpleStackNavigationManager(IMauiContext mauiContext)
    {
        this.mauiContext = mauiContext;
    }


    public virtual void NavigateTo(NavigationRequest args, SimpleShell shell, IView shellSectionContainer)
    {
        IReadOnlyList<IView> newPageStack = new List<IView>(args.NavigationStack);
        var previousNavigationStack = NavigationStack;
        var previousNavigationStackCount = previousNavigationStack.Count;
        bool initialNavigation = NavigationStack.Count == 0;

        // User has modified navigation stack but not the currently visible page
        // So we just sync the elements in the stack
        if (!initialNavigation &&
            newPageStack[newPageStack.Count - 1] == previousNavigationStack[previousNavigationStackCount - 1])
        {
            OnBackStackChanged(newPageStack);
            return;
        }

        var previousPage = currentPage;
        currentPage = newPageStack[newPageStack.Count - 1];

        var previousShellSectionContainer = currentShellSectionContainer;
        currentShellSectionContainer = shellSectionContainer;

        var isPreviousPageRoot = isCurrentPageRoot;
        isCurrentPageRoot = newPageStack.Count < 2;

        _ = currentPage ?? throw new InvalidOperationException("Navigation Request Contains Null Elements");

        var transitionType = SimpleShellTransitionType.Popping;

        if (previousNavigationStack.Count == newPageStack.Count || previousNavigationStack?.FirstOrDefault() != newPageStack?.FirstOrDefault())
            transitionType = SimpleShellTransitionType.Switching;
        else if (previousNavigationStack.Count < newPageStack.Count)
            transitionType = SimpleShellTransitionType.Pushing;

        NavigateToPage(transitionType, args, shell, newPageStack, previousShellSectionContainer, previousPage, isPreviousPageRoot);
    }

    protected abstract void NavigateToPage(
        SimpleShellTransitionType transitionType,
        NavigationRequest args,
        SimpleShell shell,
        IReadOnlyList<IView> newPageStack,
        IView previousShellSectionContainer,
        IView previousPage,
        bool isPreviousPageRoot);

    protected abstract void OnBackStackChanged(IReadOnlyList<IView> newPageStack);

    protected virtual PlatformView GetPlatformView(IView view)
    {
        return view?.ToPlatform(mauiContext);
    }

    public virtual void UpdateRootPageContainer(IView rootPageContainer)
    {
        if (NavigationStack is null)
        {
            this.rootPageContainer = rootPageContainer;
            return;
        }

        ReplaceRootPageContainer(rootPageContainer, isCurrentPageRoot);
        this.rootPageContainer = rootPageContainer;
    }

    protected virtual object GetPageContainerNavHost(IView pageContainer)
    {
        return pageContainer?.FindSimpleNavigationHost()?.Handler?.PlatformView;
    }

    protected private bool ShouldBeAbove(SimpleShellTransition transition, SimpleShellTransitionArgs args)
    {
        return transition.DestinationPageInFront?.Invoke(args) ?? args.TransitionType switch
        {
            SimpleShellTransitionType.Pushing => SimpleShellTransition.DefaultDestinationPageInFrontOnPushing,
            SimpleShellTransitionType.Popping => SimpleShellTransition.DefaultDestinationPageInFrontOnPopping,
            SimpleShellTransitionType.Switching => SimpleShellTransition.DefaultDestinationPageInFrontOnSwitching,
            _ => true
        };
    }

    protected private void FireNavigationFinished()
    {
        StackNavigation?.NavigationFinished(NavigationStack);
    }
}