using SimpleToolkit.SimpleShell.Handlers;
using SimpleToolkit.SimpleShell.Transitions;
using Microsoft.Maui.Controls.Internals;
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

namespace SimpleToolkit.SimpleShell.NavigationManager
{
    public partial class SimpleStackNavigationManager : BaseSimpleStackNavigationManager
    {
        public SimpleStackNavigationManager(IMauiContext mauiContext) : base(mauiContext) { }


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
            ArgsNavigationRequest args,
            SimpleShell shell,
            IReadOnlyList<IView> newPageStack,
            IView previousShellSectionContainer,
            IView previousPage,
            bool isPreviousPageRoot)
        {
            NavigationStack = newPageStack;

            if (args.RequestType == NavigationRequestType.Remove || args.RequestType == NavigationRequestType.Insert)
            {
                SwitchPagesInContainer(shell, previousShellSectionContainer, previousPage, isPreviousPageRoot);
                FireNavigationFinished();
                return;
            }

            NavigateToPageInContainer(transitionType, shell, previousShellSectionContainer, previousPage, isPreviousPageRoot);
        }
        
        protected override void OnBackStackChanged(IReadOnlyList<IView> newPageStack)
        {
            NavigationStack = newPageStack;
            FireNavigationFinished();
        }

        private int GetChildrenCount()
        {
#if ANDROID
            return navigationFrame.ChildCount;
#elif IOS || MACCATALYST
            return navigationFrame.Subviews.Length;
#elif WINDOWS
            return navigationFrame.Children.Count;
#endif
            return 0;
        }
    }
}
