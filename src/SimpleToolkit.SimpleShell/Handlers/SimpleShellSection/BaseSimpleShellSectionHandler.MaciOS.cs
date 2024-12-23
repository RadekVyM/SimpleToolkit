using SimpleToolkit.SimpleShell.Extensions;

namespace SimpleToolkit.SimpleShell.Handlers;

public abstract partial class BaseSimpleShellSectionHandler<PlatformT> where PlatformT : class
{
    protected void AddToParentController(UIKit.UIViewController viewController)
    {
        var shell = VirtualView.FindParentOfType<SimpleShell>() ??
            throw new NullReferenceException("Could not find a Shell instance in the view tree.");

        if (shell.Handler is not SimpleShellHandler shellHandler)
            return;

        var shellController = shellHandler.ViewController;

        shellController?.AddChildViewController(viewController);
        viewController.DidMoveToParentViewController(shellController);
    }
}