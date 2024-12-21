using SimpleToolkit.SimpleShell.Extensions;
using SimpleToolkit.SimpleShell.Platform;
using UIKit;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class PlatformSimpleShellSectionHandler
{
    public UIViewController RootContentController { get; private set; } = null!;
    public UINavigationController ViewController { get; private set; } = null!;
    protected UIView RootContentContainer => RootContentController.View ?? throw new NullReferenceException("UIViewController's View should not be null here.");

    protected override UIView CreatePlatformElement()
    {
        CreateNavigationManager();

        RootContentController = new SimpleShellSectionContentController
        {
            View = new SimpleContentView(),
        };

        var shell = VirtualView.FindParentOfType<SimpleShell>() ?? throw new NullReferenceException("Could not find a Shell instance in the view tree.");
        var navigationController = new PlatformSimpleShellSectionController(RootContentController, shell);

        ViewController = navigationController;
        AddToParentController(navigationController);

        return navigationController.View ?? throw new NullReferenceException("UIViewController's View should not be null here.");
    }
}