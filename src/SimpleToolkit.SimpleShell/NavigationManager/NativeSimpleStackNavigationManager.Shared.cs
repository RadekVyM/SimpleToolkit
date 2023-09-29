using SimpleToolkit.SimpleShell.Transitions;
using SimpleToolkit.SimpleShell.Handlers;
using Microsoft.Maui.Controls;
#if ANDROID
using NavFrame = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
using RootContainer = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
using PlatformView = Android.Views.View;
#elif __IOS__ || MACCATALYST
using UIKit;
using NavFrame = UIKit.UIView;
using RootContainer = UIKit.UIView;
using PlatformView = UIKit.UIView;
#elif WINDOWS
using NavFrame = Microsoft.UI.Xaml.Controls.Grid;
using RootContainer = Microsoft.UI.Xaml.Controls.Frame;
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using NavFrame = System.Object;
using RootContainer = System.Object;
using PlatformView = System.Object;
#endif

namespace SimpleToolkit.SimpleShell.NavigationManager;

public partial class NativeSimpleStackNavigationManager : BaseSimpleStackNavigationManager, ISimpleStackNavigationManager
{
    protected RootContainer rootContainer;


    public NativeSimpleStackNavigationManager(IMauiContext mauiContext) : base(mauiContext, true) { }


    public virtual void Connect(IStackNavigation navigationView, RootContainer rootContainer, NavFrame navigationFrame)
    {
        this.navigationFrame = navigationFrame;
        this.rootContainer = rootContainer;
        StackNavigation = navigationView;

#if WINDOWS
        rootContainer.Navigated += OnNavigated;
#endif
    }

    public virtual void Disconnect(IStackNavigation navigationView, RootContainer rootContainer, NavFrame navigationFrame)
    {
#if WINDOWS
        rootContainer.Navigated -= OnNavigated;
#endif

        this.navigationFrame = null;
        this.rootContainer = null;
        StackNavigation = null;
    }

    protected override async void NavigateToPage(
        SimpleShellTransitionType transitionType,
        PresentationMode presentationMode,
        ArgsNavigationRequest args,
        SimpleShell shell,
        IReadOnlyList<IView> newPageStack,
        IView previousShellItemContainer,
        IView previousShellSectionContainer,
        IView previousPage,
        bool isPreviousPageRoot)
    {
        var oldRootPage = NavigationStack.FirstOrDefault();
        var newRootPage = newPageStack.FirstOrDefault();
        var animated = presentationMode == PresentationMode.Animated;

        if (transitionType == SimpleShellTransitionType.Switching && isCurrentPageRoot)
        {
            HandleNewStack(newPageStack, GetTransition, CreateArgs);
            await NavigateNativelyToPageInContainer(
                shell,
                previousShellItemContainer,
                previousShellSectionContainer,
                oldRootPage,
                true,
                GetTransition,
                CreateArgs,
                animated);
            FireNavigationFinished();
            return;
        }

        if (oldRootPage != newRootPage)
        {
            // Navigating to a page with a different root page in its navigation stack
            RemovePlatformPageFromContainer(oldRootPage, previousShellItemContainer, previousShellSectionContainer, true, true);
            AddPlatformPageToContainer(newRootPage, shell, isCurrentPageRoot: true);
        }

        if (isCurrentPageRoot)
            SwitchPagesInContainer(shell, previousShellItemContainer, previousShellSectionContainer, oldRootPage, true);

        HandleNewStack(newPageStack, GetTransition, CreateArgs, !(transitionType == SimpleShellTransitionType.Switching && !isCurrentPageRoot) && animated);
        FireNavigationFinished();


        PlatformSimpleShellTransition GetTransition(IView page)
        {
            return NativeSimpleStackNavigationManager.GetTransition(page, shell);
        }

        SimpleShellTransitionArgs CreateArgs()
        {
            return new SimpleShellTransitionArgs(
                originPage: previousPage as VisualElement,
                destinationPage: currentPage as VisualElement,
                originShellSectionContainer: previousShellSectionContainer as VisualElement,
                destinationShellSectionContainer: currentShellSectionContainer as VisualElement,
                originShellItemContainer: previousShellItemContainer as VisualElement,
                destinationShellItemContainer: currentShellItemContainer as VisualElement,
                shell: shell,
                progress: 0,
                transitionType: transitionType,
                isOriginPageRoot: isPreviousPageRoot,
                isDestinationPageRoot: isCurrentPageRoot);
        }
    }

    protected override void OnBackStackChanged(IReadOnlyList<IView> newPageStack, SimpleShell shell)
    {
        var oldRootPage = NavigationStack.FirstOrDefault();
        var newRootPage = newPageStack.FirstOrDefault();

        HandleNewStack(newPageStack, GetTransition, null, false);

        if (oldRootPage != newRootPage)
        {
            // Navigating to a page with a different root page in its navigation stack
            RemovePlatformPageFromContainer(oldRootPage, currentShellItemContainer, currentShellSectionContainer, true, true);
            AddPlatformPageToContainer(newRootPage, shell, isCurrentPageRoot: true);
        }

        FireNavigationFinished();

        PlatformSimpleShellTransition GetTransition(IView page)
        {
            return NativeSimpleStackNavigationManager.GetTransition(page, shell);
        }
    }

    private static PlatformSimpleShellTransition GetTransition(IView page, SimpleShell shell)
    {
        var pageTransition = page is VisualElement visualCurrentPage ? SimpleShell.GetTransition(visualCurrentPage) : null;
        pageTransition ??= SimpleShell.GetTransition(shell);
        return pageTransition is PlatformSimpleShellTransition pt ? pt : null;
    }

    private static T GetValue<T>(
        PlatformSimpleShellTransition transition,
        Func<SimpleShellTransitionArgs> args,
        Func<SimpleShellTransitionArgs, T> value,
        T defaultValue)
    {
        if (transition is null || value is null)
            return defaultValue;

        return value(args?.Invoke());
    }

    private static T GetValue<T>(
        PlatformSimpleShellTransition transition,
        Func<T> value,
        T defaultValue)
    {
        if (transition is null || value is null)
            return defaultValue;

        return value();
    }
}