using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Handlers;
using SimpleToolkit.SimpleShell.Transitions;
using Microsoft.Maui.Controls.Internals;
using SimpleToolkit.SimpleShell.Extensions;
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
        protected IView rootPageContainer;

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
                newPageStack[newPageStack.Count - 1] == previousNavigationStack[previousNavigationStackCount - 1])
            {
                NavigationStack = newPageStack;
                FireNavigationFinished();
                return;
            }

            var previousPage = currentPage;
            currentPage = newPageStack[newPageStack.Count - 1];

            isPreviousPageRoot = isCurrentPageRoot;
            isCurrentPageRoot = newPageStack.Count < 2;

            _ = currentPage ?? throw new InvalidOperationException("Navigatoin Request Contains Null Elements");

            if (previousNavigationStack.Count == newPageStack.Count || previousNavigationStack?.FirstOrDefault() != newPageStack?.FirstOrDefault())
            {
                NavigateToPage(SimpleShellTransitionType.Switching);
            }
            else if (previousNavigationStack.Count < newPageStack.Count)
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

                if (args is ArgsNavigationRequest a && (a.RequestType == NavigationRequestType.Remove || a.RequestType == NavigationRequestType.Insert))
                {
                    RemovePlatformPage(oldPageView);
                    AddPlatformPage(newPageView);
                    FireNavigationFinished();
                    return;
                }

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

        public virtual void UpdateRootPageContainer(IView rootPageContainer)
        {
            if (NavigationStack is null)
            {
                this.rootPageContainer = rootPageContainer;
                return;
            }

            ReplaceRootPageContainer(rootPageContainer);
            this.rootPageContainer = rootPageContainer;
        }

        protected virtual PlatformView GetPlatformView(IView view)
        {
            return view?.ToPlatform(mauiContext);
        }

        protected virtual object GetRooPageContainerNavHost(IView rootPageContainer)
        {
            return rootPageContainer?.FindSimpleNavigationHost()?.Handler?.PlatformView;
        }

        private bool ShouldBeAbove(SimpleShellTransition transition, SimpleShellTransitionType transitionType, VisualElement oldPage, VisualElement newPage)
        {
            var args = new SimpleShellTransitionArgs(oldPage, newPage, 0, transitionType);

            return transition.DestinationPageInFront?.Invoke(args) ?? transitionType switch
            {
                SimpleShellTransitionType.Pushing => SimpleShellTransition.DefaultDestinationPageInFrontOnPushing,
                SimpleShellTransitionType.Popping => SimpleShellTransition.DefaultDestinationPageInFrontOnPopping,
                SimpleShellTransitionType.Switching => SimpleShellTransition.DefaultDestinationPageInFrontOnSwitching,
                _ => true
            };
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
