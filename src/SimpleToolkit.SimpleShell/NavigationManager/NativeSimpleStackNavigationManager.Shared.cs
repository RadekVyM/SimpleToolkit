using SimpleToolkit.SimpleShell.Transitions;
using SimpleToolkit.SimpleShell.Handlers;
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


    public NativeSimpleStackNavigationManager(IMauiContext mauiContext) : base(mauiContext) { }


    public virtual void Connect(IStackNavigation navigationView, RootContainer rootContainer, NavFrame navigationFrame)
    {
        this.navigationFrame = navigationFrame;
        this.rootContainer = rootContainer;
        StackNavigation = navigationView;
    }

    public virtual void Disconnect(IStackNavigation navigationView, RootContainer rootContainer, NavFrame navigationFrame)
    {
        this.navigationFrame = null;
        this.rootContainer = null;
        StackNavigation = null;
    }

    protected override void NavigateToPage(
        SimpleShellTransitionType transitionType,
        ArgsNavigationRequest args,
        SimpleShell shell,
        IReadOnlyList<IView> newPageStack,
        IView previousShellSectionContainer,
        IView previousPage,
        bool isPreviousPageRoot)
    {
        // TODO: FireNavigationFinished() should be probably called after a transition animaiton is completed

        var oldRootPage = NavigationStack.FirstOrDefault();
        NavigationStack = newPageStack;

        if (transitionType == SimpleShellTransitionType.Switching && isCurrentPageRoot)
        {
            HandleNewStack(newPageStack);
            NavigateToPageInContainer(transitionType, shell, previousShellSectionContainer, previousPage, isPreviousPageRoot);
            return;
        }

        if (isCurrentPageRoot)
            SwitchPagesInContainer(shell, previousShellSectionContainer, oldRootPage, true);

        HandleNewStack(newPageStack, !(transitionType == SimpleShellTransitionType.Switching && !isCurrentPageRoot));
        FireNavigationFinished();
    }
}