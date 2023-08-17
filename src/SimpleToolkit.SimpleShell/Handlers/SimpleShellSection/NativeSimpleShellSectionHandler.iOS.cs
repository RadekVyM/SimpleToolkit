#if IOS || MACCATALYST

using SimpleToolkit.SimpleShell.Platform;
using UIKit;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class NativeSimpleShellSectionHandler
{
    public UIViewController RootContentController { get; private set; }
    public UINavigationController ViewController { get; private set; }
    protected UIView RootContentContainer => RootContentController.View;

    protected override UIView CreatePlatformElement()
    {
        CreateNavigationManager();

        RootContentController = new SimpleShellSectionContentController
        {
            View = new SimpleContentView(),
        };

        var navigationController = new NativeSimpleShellSectionController(RootContentController);

        ViewController = navigationController;
        AddToParentController(navigationController);

        return navigationController.View;
    }
}

#endif