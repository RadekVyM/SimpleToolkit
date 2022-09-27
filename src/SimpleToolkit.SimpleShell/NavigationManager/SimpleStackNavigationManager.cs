using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Transitions;
using System;
using System.Linq.Expressions;
#if ANDROID
using NavFrame = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
using PlatformPage = Android.Views.View;
#elif __IOS__ || MACCATALYST
using UIKit;
using NavFrame = UIKit.UIView;
using PlatformPage = UIKit.UIView;
#elif WINDOWS
using NavFrame = Microsoft.UI.Xaml.Controls.Grid;
using PlatformPage = Microsoft.UI.Xaml.FrameworkElement;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using NavFrame = System.Object;
using PlatformPage = System.Object;
#endif

namespace SimpleToolkit.SimpleShell.NavigationManager
{
    public class SimpleStackNavigationManager
    {
        protected const string TransitionAnimationKey = nameof(SimpleShellTransition);

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

            var previousPage = currentPage;
            currentPage = newPageStack[newPageStack.Count - 1];

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
                var oldPageView = previousPage is not null ? GetPageView(previousPage) : null;
                var newPageView = GetPageView(currentPage);
                NavigationStack = newPageStack;

                var transition = currentPage is VisualElement visualCurrentPage ? SimpleShell.GetTransition(visualCurrentPage) : null;
                transition ??= SimpleShell.GetTransition(Shell.Current);

                if (transition is not null && currentPage is VisualElement visualCurrent && previousPage is VisualElement visualPrevious)
                {
                    var animation = new Animation(progress =>
                    {
                        transition.Callback?.Invoke(new SimpleShellTransitionArgs(visualPrevious, visualCurrent, progress, transitionType));
                    });

                    visualPrevious.AbortAnimation(TransitionAnimationKey);

                    transition.Starting?.Invoke(new SimpleShellTransitionArgs(visualPrevious, visualCurrent, 0, transitionType));

                    AddPlatformPage(newPageView, ShouldBeAbove(transition, transitionType));

                    animation.Commit(visualCurrent, TransitionAnimationKey, length: transition.Duration, finished: (v, canceled) =>
                    {
                        RemovePlatformPage(oldPageView);
                        transition.Finished?.Invoke(new SimpleShellTransitionArgs(visualPrevious, visualCurrent, v, transitionType));

                        FireNavigationFinished();
                        var count = GetChildrenCount();
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

        protected virtual PlatformPage GetPageView(IView page)
        {
            var pageView = page.ToPlatform(mauiContext);
            return pageView;
        }

        protected virtual void AddPlatformPage(PlatformPage newPageView, bool onTop = true)
        {
#if ANDROID
            navigationFrame.AddView(newPageView);
            if (!onTop)
                navigationFrame.BringChildToFront(navigationFrame.GetChildAt(0));
#elif IOS || MACCATALYST
            navigationFrame.AddSubview(newPageView);

            if (!onTop)
                navigationFrame.BringSubviewToFront(navigationFrame.Subviews.FirstOrDefault());
#elif WINDOWS
            if (onTop)
                navigationFrame.Children.Add(newPageView);
            else
                navigationFrame.Children.Insert(0, newPageView);
#endif
        }

        protected virtual void RemovePlatformPage(PlatformPage oldPageView)
        {
#if ANDROID
            if (oldPageView is not null)
                navigationFrame.RemoveView(oldPageView);
#elif IOS || MACCATALYST
            oldPageView?.RemoveFromSuperview();
#elif WINDOWS
            navigationFrame.Children.Remove(oldPageView);
#endif
        }

        private bool ShouldBeAbove(SimpleShellTransition transition, SimpleShellTransitionType transitionType)
        {
            return (transitionType == SimpleShellTransitionType.Pushing && transition.DestinationPageAboveOnPushing)
                || (transitionType == SimpleShellTransitionType.Popping && transition.DestinationPageAboveOnPopping)
                || (transitionType == SimpleShellTransitionType.Switching && transition.DestinationPageAboveOnSwitching);
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
