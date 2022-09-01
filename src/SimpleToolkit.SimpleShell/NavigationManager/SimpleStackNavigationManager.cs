using Microsoft.Maui.Platform;
#if ANDROID
using NavFrame = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
using PlatformPage = Android.Views.View;
#elif __IOS__ || MACCATALYST
using UIKit;
using NavFrame = UIKit.UIView;
using PlatformPage = UIKit.UIView;
#elif WINDOWS
using NavFrame = Microsoft.UI.Xaml.Controls.Frame;
using PlatformPage = Microsoft.UI.Xaml.FrameworkElement;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using NavFrame = System.Object;
using PlatformPage = System.Object;
#endif

namespace SimpleToolkit.SimpleShell.NavigationManager
{
    public class SimpleStackNavigationManager
    {
        protected IMauiContext mauiContext;
        protected NavFrame navigationFrame;
        protected IView currentPage;

        public IStackNavigation StackNavigation { get; protected set; }
        public IReadOnlyList<IView> NavigationStack { get; protected set; } = new List<IView>();

        public SimpleStackNavigationManager(IMauiContext mauiContext)
        {
            this.mauiContext = mauiContext;
        }

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

        public virtual void NavigateTo(NavigationRequest args)
        {
            IReadOnlyList<IView> newPageStack = new List<IView>(args.NavigationStack);
            var previousNavigationStack = NavigationStack;
            var previousNavigationStackCount = previousNavigationStack.Count;
            bool initialNavigation = NavigationStack.Count == 0;

            // User has modified navigation stack but not the currently visible page
            // So we just sync the elements in the stack
            if (!initialNavigation &&
                newPageStack[newPageStack.Count - 1] ==
                previousNavigationStack[previousNavigationStackCount - 1])
            {
                NavigationStack = newPageStack;
                FireNavigationFinished();
                return;
            }

            currentPage = newPageStack[newPageStack.Count - 1];

            _ = currentPage ?? throw new InvalidOperationException("Navigatoin Request Contains Null Elements");
            // Prepared for potential animations
            if (previousNavigationStack.Count < args.NavigationStack.Count)
            {
                NavigateToPage();
            }
            else if (previousNavigationStack.Count == args.NavigationStack.Count)
            {
                NavigateToPage();
            }
            else
            {
                NavigateToPage();
            }

            FireNavigationFinished();

            void NavigateToPage()
            {
                var pageView = GetPageView(currentPage);
                NavigationStack = newPageStack;

#if ANDROID
                navigationFrame.RemoveAllViews();
                navigationFrame.AddView(pageView);
#elif __IOS__ || MACCATALYST
                navigationFrame.ClearSubviews();
                navigationFrame.AddSubview(pageView);
#elif WINDOWS
                navigationFrame.Content = pageView;
#endif
            }
        }

        protected virtual PlatformPage GetPageView(IView page)
        {
            var pageView = page.ToPlatform(mauiContext);
            return pageView;
        }

        void FireNavigationFinished()
        {
            StackNavigation?.NavigationFinished(NavigationStack);
        }
    }
}
