#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Platform;
using UIKit;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public partial class NativeSimpleStackNavigationManager
{
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