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
        protected const string TransitionAnimationKey = nameof(SimpleShellTransition);

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
            NavigationRequest args,
            SimpleShell shell,
            IReadOnlyList<IView> newPageStack,
            IView previousShellSectionContainer,
            IView previousPage,
            bool isPreviousPageRoot)
        {
            var oldPageView = previousPage is not null ? GetPlatformView(previousPage) : null;
            NavigationStack = newPageStack;

            var transition = currentPage is VisualElement visualCurrentPage ? SimpleShell.GetTransition(visualCurrentPage) : null;
            transition ??= SimpleShell.GetTransition(shell);

            if (args is ArgsNavigationRequest a && (a.RequestType == NavigationRequestType.Remove || a.RequestType == NavigationRequestType.Insert))
            {
                RemovePlatformPage(previousPage, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);
                AddPlatformPage(currentPage, shell, isCurrentPageRoot: isCurrentPageRoot);
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
                AddPlatformPage(currentPage, shell, ShouldBeAbove(transition, CreateArgs(visualCurrent, visualPrevious, transitionType, 0)), isCurrentPageRoot: isCurrentPageRoot);

                transition.Starting?.Invoke(CreateArgs(visualCurrent, visualPrevious, transitionType, 0));

                var duration = transition.Duration?.Invoke(CreateArgs(visualCurrent, visualPrevious, transitionType, 0)) ?? SimpleShellTransition.DefaultDuration;
                var easing = transition.Easing?.Invoke(CreateArgs(visualCurrent, visualPrevious, transitionType, 0)) ?? Easing.Linear;

                animation.Commit(
                    visualCurrent,
                    name: TransitionAnimationKey,
                    length: duration,
                    easing: easing,
                    finished: (v, canceled) =>
                    {
                        RemovePlatformPage(previousPage, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);
                        transition.Finished?.Invoke(CreateArgs(visualCurrent, visualPrevious, transitionType, v));

                        FireNavigationFinished();
                    });
            }
            else
            {
                RemovePlatformPage(previousPage, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);
                AddPlatformPage(currentPage, shell, isCurrentPageRoot: isCurrentPageRoot);
                FireNavigationFinished();
            }

            SimpleShellTransitionArgs CreateArgs(VisualElement visualCurrent, VisualElement visualPrevious, SimpleShellTransitionType transitionType, double progress)
            {
                return new SimpleShellTransitionArgs(
                    originPage: visualPrevious,
                    destinationPage: visualCurrent,
                    originShellSectionContainer: previousShellSectionContainer as VisualElement,
                    destinationShellSectionContainer: currentShellSectionContainer as VisualElement,
                    shell: shell,
                    progress: progress,
                    transitionType: transitionType,
                    isOriginPageRoot: isPreviousPageRoot,
                    isDestinationPageRoot: isCurrentPageRoot);
            }
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
