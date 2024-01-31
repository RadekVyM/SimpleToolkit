using SimpleToolkit.SimpleShell.Handlers;
using SimpleToolkit.SimpleShell.Transitions;
using Microsoft.Maui.Controls.Internals;
#if ANDROID
using NavFrame = Android.Widget.FrameLayout;
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

public partial class SimpleStackNavigationManager : BaseSimpleStackNavigationManager
{
    public SimpleStackNavigationManager(IMauiContext mauiContext) : base(mauiContext, false) { }


    public virtual void Connect(IStackNavigation navigationView, NavFrame navigationFrame)
    {
        this.navigationFrame = navigationFrame;
        StackNavigation = navigationView;
    }

    public virtual void Disconnect(IStackNavigation navigationView, NavFrame navigationFrame)
    {
        this.navigationFrame = null;
        StackNavigation = null;
    }

    protected override void NavigateToPage(
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
        var oldPageStack = NavigationStack;
        NavigationStack = newPageStack;

        if (args.RequestType == NavigationRequestType.Remove || args.RequestType == NavigationRequestType.Insert)
        {
            SwitchPagesInContainer(shell, previousShellItemContainer, previousShellSectionContainer, previousPage, isPreviousPageRoot);
            FireNavigationFinished();
            return;
        }

        var pagesToDisconnect = GetPagesToDisconnect(oldPageStack, newPageStack);

        NavigateToPageInContainer(transitionType, presentationMode, shell, previousShellItemContainer, previousShellSectionContainer, previousPage, isPreviousPageRoot, pagesToDisconnect);
    }
    
    protected override void OnBackStackChanged(IReadOnlyList<IView> newPageStack, SimpleShell shell)
    {
        NavigationStack = newPageStack;
        FireNavigationFinished();
    }
}