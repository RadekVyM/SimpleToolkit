#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Platform;
using UIKit;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public partial class NativeSimpleStackNavigationManager
{
    protected Task NavigateNativelyToPageInContainer(
        SimpleShell shell,
        IView previousShellSectionContainer,
        IView previousPage,
        bool isPreviousPageRoot)
    {
        AddPlatformPageToContainer(currentPage, shell, false, isCurrentPageRoot: isCurrentPageRoot);

        var newPageView = GetPlatformView(currentPage);
        var oldPageView = GetPlatformView(previousPage);
        var newSectionContainer = GetPlatformView(currentShellSectionContainer);
        var oldSectionContainer = GetPlatformView(previousShellSectionContainer);

        var to = newSectionContainer == oldSectionContainer ?
            newPageView :
            newSectionContainer ?? newPageView;
        var from = newSectionContainer == oldSectionContainer ?
            oldPageView :
            oldSectionContainer ?? oldPageView;

        if (from is not null)
        {
            UIView.Transition(from, to, 0.2, UIViewAnimationOptions.TransitionCrossDissolve, () =>
                RemovePlatformPageFromContainer(previousPage, previousShellSectionContainer, isCurrentPageRoot, isPreviousPageRoot));
        }

        return Task.CompletedTask;
    }

    protected void HandleNewStack(IReadOnlyList<IView> newPageStack, bool animated = true)
    {
        NavigationStack = newPageStack;
        var controller = rootContainer.NextResponder as NativeSimpleShellSectionController;
        var root = navigationFrame.NextResponder as UIViewController;

        var newControllers = newPageStack
            .Skip(1)
            .Select(p =>
            {
                if (p.ToHandler(mauiContext) is not PageHandler pageHandler)
                    throw new InvalidOperationException("Handler of a page which you are navigating to is not inhereted from PageHandler");

                return pageHandler.ViewController;
            })
            .Prepend(root)
            .ToArray();

        controller.HandleNewStack(newControllers, animated);
    }
}

#endif