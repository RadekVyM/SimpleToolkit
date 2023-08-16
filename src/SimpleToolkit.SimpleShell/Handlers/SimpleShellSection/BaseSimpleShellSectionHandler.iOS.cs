#if IOS || MACCATALYST

using SimpleToolkit.SimpleShell.Extensions;

namespace SimpleToolkit.SimpleShell.Handlers;

public abstract partial class BaseSimpleShellSectionHandler<PlatformT> where PlatformT : class
{
    protected void AddToParentController(UIKit.UIViewController viewController)
    {
        var shell = VirtualView.FindParentOfType<SimpleShell>();

        if (shell.Handler is not SimpleShellHandler shellHandler)
            return;

        var shellController = shellHandler.ViewController;

        shellController?.AddChildViewController(viewController);
        viewController.DidMoveToParentViewController(shellController);
    }
}

#endif