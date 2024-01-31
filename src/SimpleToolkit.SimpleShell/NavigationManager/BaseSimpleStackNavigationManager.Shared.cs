using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Extensions;
using SimpleToolkit.SimpleShell.Handlers;
using SimpleToolkit.SimpleShell.Transitions;
#if ANDROID
using NavFrame = Android.Widget.FrameLayout;
using PlatformView = Android.Views.View;
using PlatformContainer = Android.Views.ViewGroup;
using PlatformChild = Android.Views.View;
#elif __IOS__ || MACCATALYST
using UIKit;
using NavFrame = UIKit.UIView;
using PlatformView = UIKit.UIView;
using PlatformContainer = UIKit.UIView;
using PlatformChild = UIKit.UIView;
#elif WINDOWS
using NavFrame = Microsoft.UI.Xaml.Controls.Grid;
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
using PlatformContainer = Microsoft.UI.Xaml.Controls.Panel;
using PlatformChild = Microsoft.UI.Xaml.UIElement;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using NavFrame = System.Object;
using PlatformView = System.Object;
using PlatformContainer = System.Object;
using PlatformChild = System.Object;
#endif

namespace SimpleToolkit.SimpleShell.NavigationManager;

public abstract partial class BaseSimpleStackNavigationManager : ISimpleStackNavigationManager
{
    protected const string TransitionAnimationKey = nameof(SimpleShellTransition);

    private readonly bool alwaysAddRootPageContainerBackWhenReplaced;
    protected IMauiContext mauiContext;
    protected NavFrame navigationFrame;
    protected IView currentPage;
    protected IView rootPageContainer;
    protected IView currentShellSectionContainer;
    protected IView currentShellItemContainer;
    protected bool isCurrentPageRoot = true;

    public IStackNavigation StackNavigation { get; protected set; }
    public IReadOnlyList<IView> NavigationStack { get; protected set; } = new List<IView>();

    public bool AlreadyNavigated { get; private protected set; } = false;


    public BaseSimpleStackNavigationManager(IMauiContext mauiContext, bool alwaysAddRootPageContainerBackWhenReplaced)
    {
        this.mauiContext = mauiContext;
        this.alwaysAddRootPageContainerBackWhenReplaced = alwaysAddRootPageContainerBackWhenReplaced;
    }


    #region Navigation

    public virtual void NavigateTo(ArgsNavigationRequest args, SimpleShell shell, IView shellSectionContainer, IView shellItemContainer)
    {
        AlreadyNavigated = true;

        IReadOnlyList<IView> newPageStack = new List<IView>(args.NavigationStack);
        var previousNavigationStack = NavigationStack;
        var previousNavigationStackCount = previousNavigationStack.Count;
        bool initialNavigation = NavigationStack.Count == 0;

        // User has modified navigation stack but not the currently visible page
        // So we just sync the elements in the stack
        if (!initialNavigation &&
            newPageStack[newPageStack.Count - 1] == previousNavigationStack[previousNavigationStackCount - 1])
        {
            OnBackStackChanged(newPageStack, shell);
            return;
        }

        var previousPage = currentPage;
        currentPage = newPageStack[newPageStack.Count - 1];

        var previousShellSectionContainer = currentShellSectionContainer;
        currentShellSectionContainer = shellSectionContainer;

        var previousShellItemContainer = currentShellItemContainer;
        currentShellItemContainer = shellItemContainer;

        var isPreviousPageRoot = isCurrentPageRoot;
        isCurrentPageRoot = newPageStack.Count < 2;

        _ = currentPage ?? throw new InvalidOperationException("Navigation Request Contains Null Elements");

        var transitionType = SimpleShellTransitionType.Popping;

        if (previousNavigationStack.Count == newPageStack.Count || previousNavigationStack?.FirstOrDefault() != newPageStack?.FirstOrDefault())
            transitionType = SimpleShellTransitionType.Switching;
        else if (previousNavigationStack.Count < newPageStack.Count)
            transitionType = SimpleShellTransitionType.Pushing;

        var presentationMode = PresentationMode.NotAnimated;
        if (currentPage is BindableObject bindableCurrentPage)
            presentationMode = Shell.GetPresentationMode(bindableCurrentPage);

        NavigateToPage(transitionType, presentationMode, args, shell, newPageStack, previousShellItemContainer, previousShellSectionContainer, previousPage, isPreviousPageRoot);
    }

    protected private void NavigateToPageInContainer(
        SimpleShellTransitionType transitionType,
        PresentationMode presentationMode,
        SimpleShell shell,
        IView previousShellItemContainer,
        IView previousShellSectionContainer,
        IView previousPage,
        bool isPreviousPageRoot,
        IList<IView> pagesToDisconnect = null)
    {
        var pageTransition = currentPage is VisualElement visualCurrentPage ? SimpleShell.GetTransition(visualCurrentPage) : null;
        pageTransition ??= SimpleShell.GetTransition(shell);

        if (
            presentationMode == PresentationMode.Animated &&
            pageTransition is SimpleShellTransition transition &&
            currentPage is VisualElement visualCurrent &&
            previousPage is VisualElement visualPrevious)
        {
            var animation = new Animation(progress =>
            {
                transition.Callback?.Invoke(CreateArgs(progress));
            });

            visualPrevious.AbortAnimation(TransitionAnimationKey);
            AddPlatformPageToContainer(currentPage, shell, ShouldBeAbove(transition, CreateArgs(0)), isCurrentPageRoot: isCurrentPageRoot);

            transition.Starting?.Invoke(CreateArgs(0));

            var duration = transition.Duration?.Invoke(CreateArgs(0)) ?? SimpleShellTransition.DefaultDuration;
            var easing = transition.Easing?.Invoke(CreateArgs(0)) ?? Easing.Linear;

            animation.Commit(
                visualCurrent,
                name: TransitionAnimationKey,
                length: duration,
                easing: easing,
                finished: (v, canceled) =>
                {
                    RemovePlatformPageFromContainer(previousPage, previousShellItemContainer, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);
                    transition.Finished?.Invoke(CreateArgs(v));

                    FireNavigationFinished();

                    if (pagesToDisconnect is not null && pagesToDisconnect.Count > 0)
                        DisconnectHandlers(pagesToDisconnect);
                });

            SimpleShellTransitionArgs CreateArgs(double progress)
            {
                return new SimpleShellTransitionArgs(
                    originPage: visualPrevious,
                    destinationPage: visualCurrent,
                    originShellSectionContainer: previousShellSectionContainer as VisualElement,
                    destinationShellSectionContainer: currentShellSectionContainer as VisualElement,
                    originShellItemContainer: previousShellItemContainer as VisualElement,
                    destinationShellItemContainer: currentShellItemContainer as VisualElement,
                    shell: shell,
                    progress: progress,
                    transitionType: transitionType,
                    isOriginPageRoot: isPreviousPageRoot,
                    isDestinationPageRoot: isCurrentPageRoot);
            }
        }
        else
        {
            SwitchPagesInContainer(shell, previousShellItemContainer, previousShellSectionContainer, previousPage, isPreviousPageRoot);
            FireNavigationFinished();

            if (pagesToDisconnect is not null && pagesToDisconnect.Count > 0)
                DisconnectHandlers(pagesToDisconnect);
        }
    }

    protected private void SwitchPagesInContainer(
        SimpleShell shell,
        IView previousShellItemContainer,
        IView previousShellSectionContainer,
        IView previousPage,
        bool isPreviousPageRoot)
    {
        RemovePlatformPageFromContainer(previousPage, previousShellItemContainer, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot);
        AddPlatformPageToContainer(currentPage, shell, isCurrentPageRoot: isCurrentPageRoot);
    }

    protected abstract void NavigateToPage(
        SimpleShellTransitionType transitionType,
        PresentationMode presentationMode,
        ArgsNavigationRequest args,
        SimpleShell shell,
        IReadOnlyList<IView> newPageStack,
        IView previousShellItemContainer,
        IView previousShellSectionContainer,
        IView previousPage,
        bool isPreviousPageRoot);

    protected abstract void OnBackStackChanged(IReadOnlyList<IView> newPageStack, SimpleShell shell);

    #endregion

    public virtual void UpdateRootPageContainer(IView rootPageContainer)
    {
        if (NavigationStack is null)
        {
            this.rootPageContainer = rootPageContainer;
            return;
        }

        if (this.rootPageContainer != rootPageContainer)
        {
            ReplaceRootPageContainer(rootPageContainer, alwaysAddRootPageContainerBackWhenReplaced || isCurrentPageRoot);
            this.rootPageContainer = rootPageContainer;
        }
    }

    public void UpdateGroupContainers(SimpleShell shell, IView shellSectionContainer, IView shellItemContainer)
    {
        if (currentPage is null || !isCurrentPageRoot || (shellSectionContainer == currentShellSectionContainer && shellItemContainer == currentShellItemContainer))
            return;

        var previousShellSectionContainer = currentShellSectionContainer;
        currentShellSectionContainer = shellSectionContainer;

        var previousShellItemContainer = currentShellItemContainer;
        currentShellItemContainer = shellItemContainer;

        SwitchPagesInContainer(shell, previousShellItemContainer, previousShellSectionContainer, currentPage, isCurrentPageRoot);
    }

    protected private bool ShouldBeAbove(SimpleShellTransition transition, SimpleShellTransitionArgs args)
    {
        return transition.DestinationPageInFront?.Invoke(args) ?? args.TransitionType switch
        {
            SimpleShellTransitionType.Pushing => SimpleShellTransition.DefaultDestinationPageInFrontOnPushing,
            SimpleShellTransitionType.Popping => SimpleShellTransition.DefaultDestinationPageInFrontOnPopping,
            SimpleShellTransitionType.Switching => SimpleShellTransition.DefaultDestinationPageInFrontOnSwitching,
            _ => true
        };
    }

    protected private void FireNavigationFinished()
    {
        StackNavigation?.NavigationFinished(NavigationStack);
    }

    protected static T GetFirstDifferent<T>(T item, T section, T page, T differentItem, T differentSection) where T : class
    {
        return item == differentItem ?
            (section == differentSection ?
                page :
                section ?? page) :
                item ?? section ?? page;
    }

    protected private static IList<IView> GetPagesToDisconnect(IEnumerable<IView> oldPageStack, IEnumerable<IView> newPageStack)
    {
        return oldPageStack.Skip(1).Except(newPageStack).ToList();
    }

    protected static void DisconnectHandlers(IEnumerable<IView> oldPageStack, IEnumerable<IView> newPageStack)
    {
        var pageStack = GetPagesToDisconnect(oldPageStack, newPageStack);
        DisconnectHandlers(pageStack);
    }

    protected static void DisconnectHandlers(IEnumerable<IView> pageStack)
    {
        foreach (var page in pageStack)
        {
            if (page is not BindableObject bindablePage || !SimpleShell.GetShouldAutoDisconnectPageHandler(bindablePage))
                return;

            // Wait until the page is unloaded
            if (page is VisualElement v && v.IsLoaded)
                v.Unloaded += OnPageUnloaded;
            else
                DisconnectHandler(page);
        }

        static void OnPageUnloaded(object sender, EventArgs e)
        {
            var page = sender as VisualElement;
            page.Unloaded -= OnPageUnloaded;
            DisconnectHandler(page);
        }

        static void DisconnectHandler(IView page)
        {
            var handler = page.Handler;
            handler?.DisconnectHandler();
        }
    }

    #region Platform views manipulation

    protected virtual PlatformView GetPlatformView(IView view)
    {
#if IOS || MACCATALYST
        // The ToPlatform() method does not return the actual view of a page controller
        // This causes problems with page backgrounds which were not present before following PR was merged:
        // https://github.com/dotnet/maui/pull/15832/files
        var controller = view?.ToUIViewController(mauiContext);

        if (controller is PageViewController pc)
            return pc.View;
#endif

        return view?.ToPlatform(mauiContext);
    }

    protected virtual object GetPageContainerNavHost(IView pageContainer)
    {
        return pageContainer?.FindSimpleNavigationHost()?.Handler?.PlatformView;
    }

    protected virtual void ReplaceRootPageContainer(IView newRootPageContainer, bool isCurrentPageRoot)
    {
        ReplaceContainer(this.rootPageContainer, newRootPageContainer, navigationFrame, isCurrentPageRoot);
    }

    private void ReplaceContainer(IView oldContainer, IView newContainer, PlatformContainer parent, bool isCurrentPageRoot)
    {
        var newPlatformContainer = GetPlatformView(newContainer);
        List<PlatformChild> oldChildren = new List<PlatformChild>();

        if (oldContainer is not null)
            oldChildren.AddRange(RemoveContainer(oldContainer, parent));

        // Old container is being replaced or added
        if (newPlatformContainer is not null && isCurrentPageRoot)
        {
            // New container is being added
            if (oldContainer is null)
                ClearChildren(parent, oldChildren);

            AddChild(parent, newPlatformContainer);

            if (GetPageContainerNavHost(newContainer) is PlatformContainer newNavHost)
            {
                foreach (var child in oldChildren)
                    AddChild(newNavHost, child);
            }
        }

        // Old container is being removed
        if (oldContainer is not null && newPlatformContainer is null && isCurrentPageRoot)
        {
            foreach (var child in oldChildren)
                AddChild(parent, child);
        }
    }

    protected private partial List<PlatformChild> RemoveContainer(IView oldContainer, PlatformContainer parent = null);

    private static partial void AddChild(PlatformContainer parent, PlatformChild child);

    private static partial void ClearChildren(PlatformContainer parent, List<PlatformChild> oldChildren);

    #endregion
}