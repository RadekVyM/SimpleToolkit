using Microsoft.Maui.Platform;
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

namespace SimpleToolkit.SimpleShell.NavigationManager
{
    public partial class SimpleStackNavigationManager
    {
        protected const string TransitionAnimationKey = nameof(SimpleShellTransition);

        private bool isCurrentPageRoot = true;
        private bool isPreviousPageRoot = false;

        protected IMauiContext mauiContext;
        protected NavFrame navigationFrame;
        protected IView currentPage;
        protected IView rootPageOverlay;

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

            var previousPage = currentPage;
            currentPage = newPageStack[newPageStack.Count - 1];

            isPreviousPageRoot = isCurrentPageRoot;
            isCurrentPageRoot = args.NavigationStack.Count < 2;

            _ = currentPage ?? throw new InvalidOperationException("Navigatoin Request Contains Null Elements");

            if (previousNavigationStack.Count == args.NavigationStack.Count || previousNavigationStack?.FirstOrDefault() != args.NavigationStack?.FirstOrDefault())
            {
                NavigateToPage(SimpleShellTransitionType.Switching);
            }
            else if (previousNavigationStack.Count < args.NavigationStack.Count)
            {
                NavigateToPage(SimpleShellTransitionType.Pushing);
            }
            else
            {
                NavigateToPage(SimpleShellTransitionType.Popping);
            }

            void NavigateToPage(SimpleShellTransitionType transitionType)
            {
                var oldPageView = previousPage is not null ? GetPlatformView(previousPage) : null;
                var newPageView = GetPlatformView(currentPage);
                NavigationStack = newPageStack;

                var transition = currentPage is VisualElement visualCurrentPage ? SimpleShell.GetTransition(visualCurrentPage) : null;
                transition ??= SimpleShell.GetTransition(SimpleShell.Current);

                if (transition is not null && currentPage is VisualElement visualCurrent && previousPage is VisualElement visualPrevious)
                {
                    var animation = new Animation(progress =>
                    {
                        transition.Callback?.Invoke(new SimpleShellTransitionArgs(visualPrevious, visualCurrent, progress, transitionType));
                    });

                    visualPrevious.AbortAnimation(TransitionAnimationKey);
                    transition.Starting?.Invoke(new SimpleShellTransitionArgs(visualPrevious, visualCurrent, 0, transitionType));

                    AddPlatformPage(newPageView, ShouldBeAbove(transition, transitionType, visualPrevious, visualCurrent));

                    var duration = transition.Duration?.Invoke(new SimpleShellTransitionArgs(visualPrevious, visualCurrent, 0, transitionType)) ?? SimpleShellTransition.DefaultDuration;
                    
                    animation.Commit(visualCurrent, TransitionAnimationKey, length: duration, finished: (v, canceled) =>
                    {
                        RemovePlatformPage(oldPageView);
                        transition.Finished?.Invoke(new SimpleShellTransitionArgs(visualPrevious, visualCurrent, v, transitionType));

                        FireNavigationFinished();
                    });
                }
                else
                {
                    RemovePlatformPage(oldPageView);
                    AddPlatformPage(newPageView);
                    FireNavigationFinished();
                }
            }
        }

        public virtual void UpdateRootPageOverlay(IView rootPageOverlay)
        {
            if (NavigationStack is null)
            {
                this.rootPageOverlay = rootPageOverlay;
                return;
            }

            ReplaceRootPageOverlay(rootPageOverlay);
            this.rootPageOverlay = rootPageOverlay;
        }

        protected virtual PlatformView GetPlatformView(IView view)
        {
            var handler = view?.ToHandler(mauiContext);

            return handler?.ContainerView ?? handler?.PlatformView;
        }

        private bool ShouldBeAbove(SimpleShellTransition transition, SimpleShellTransitionType transitionType, VisualElement oldPage, VisualElement newPage)
        {
            var args = new SimpleShellTransitionArgs(oldPage, newPage, 0, transitionType);

            return (transitionType == SimpleShellTransitionType.Pushing && (transition.DestinationPageAboveOnPushing?.Invoke(args) ?? SimpleShellTransition.DefaultDestinationPageAboveOnPushing))
                || (transitionType == SimpleShellTransitionType.Popping && (transition.DestinationPageAboveOnPopping?.Invoke(args) ?? SimpleShellTransition.DefaultDestinationPageAboveOnPopping))
                || (transitionType == SimpleShellTransitionType.Switching && (transition.DestinationPageAboveOnSwitching?.Invoke(args) ?? SimpleShellTransition.DefaultDestinationPageAboveOnSwitching));
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

        private void FireNavigationFinished()
        {
            StackNavigation?.NavigationFinished(NavigationStack);
        }
    }
}
