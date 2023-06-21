using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Handlers;
using SimpleToolkit.SimpleShell.Transitions;
using Microsoft.Maui.Controls.Internals;
using SimpleToolkit.SimpleShell.Extensions;
using System;
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

        protected IMauiContext mauiContext;
        protected NavFrame navigationFrame;
        protected IView currentPage;
        protected IView rootPageContainer;
        protected IView currentShellSectionContainer;
        protected bool isCurrentPageRoot = true;

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

        public virtual void NavigateTo(NavigationRequest args, IView shellSectionContainer)
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

            var previousShellSectionContainer = currentShellSectionContainer;
            currentShellSectionContainer = shellSectionContainer;

            var isPreviousPageRoot = isCurrentPageRoot;
            isCurrentPageRoot = newPageStack.Count < 2;

            _ = currentPage ?? throw new InvalidOperationException("Navigation Request Contains Null Elements");

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
                    RemovePlatformPage(oldPageView, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);
                    AddPlatformPage(newPageView, isCurrentPageRoot: isCurrentPageRoot);
                    FireNavigationFinished();
                    return;
                }

                if (transition is not null && currentPage is VisualElement visualCurrent && previousPage is VisualElement visualPrevious)
                {
                    var animation = new Animation(progress =>
                    {
                        transition.Callback?.Invoke(CreateArgs(visualCurrent, visualPrevious, transitionType, progress));
                    });

                    visualPrevious.AbortAnimation(TransitionAnimationKey);
                    transition.Starting?.Invoke(CreateArgs(visualCurrent, visualPrevious, transitionType, 0));

                    AddPlatformPage(newPageView, ShouldBeAbove(transition, CreateArgs(visualCurrent, visualPrevious, transitionType, 0)), isCurrentPageRoot: isCurrentPageRoot);

                    var duration = transition.Duration?.Invoke(CreateArgs(visualCurrent, visualPrevious, transitionType, 0)) ?? SimpleShellTransition.DefaultDuration;
                    var easing = transition.Easing?.Invoke(CreateArgs(visualCurrent, visualPrevious, transitionType, 0)) ?? Easing.Linear;

                    animation.Commit(
                        visualCurrent,
                        name: TransitionAnimationKey,
                        length: duration,
                        easing: easing,
                        finished: (v, canceled) =>
                        {
                            RemovePlatformPage(oldPageView, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);
                            transition.Finished?.Invoke(CreateArgs(visualCurrent, visualPrevious, transitionType, v));

                            FireNavigationFinished();
                        });
                }
                else
                {
                    RemovePlatformPage(oldPageView, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);
                    AddPlatformPage(newPageView, isCurrentPageRoot: isCurrentPageRoot);
                    FireNavigationFinished();
                }

                SimpleShellTransitionArgs CreateArgs(VisualElement visualCurrent, VisualElement visualPrevious, SimpleShellTransitionType transitionType, double progress)
                {
                    return new SimpleShellTransitionArgs(
                        originPage: visualPrevious,
                        destinationPage: visualCurrent,
                        originShellSectionContainer: previousShellSectionContainer as VisualElement,
                        destinationShellSectionContainer: currentShellSectionContainer as VisualElement,
                        progress: progress,
                        transitionType: transitionType,
                        isOriginPageRoot: isPreviousPageRoot,
                        isDestinationPageRoot: isCurrentPageRoot);
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

            ReplaceRootPageContainer(rootPageContainer, isCurrentPageRoot);
            this.rootPageContainer = rootPageContainer;
        }

        protected virtual PlatformView GetPlatformView(IView view)
        {
            return view?.ToPlatform(mauiContext);
        }

        protected virtual object GetPageContainerNavHost(IView pageContainer)
        {
            return pageContainer?.FindSimpleNavigationHost()?.Handler?.PlatformView;
        }

        private bool ShouldBeAbove(SimpleShellTransition transition, SimpleShellTransitionArgs args)
        {
            return transition.DestinationPageInFront?.Invoke(args) ?? args.TransitionType switch
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
