#if IOS || MACCATALYST

using SimpleToolkit.SimpleShell.Platform;
using UIKit;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class NativeSimpleShellSectionHandler
{
    public UIViewController ContentController { get; private set; }
    public UINavigationController ViewController { get; private set; }

    protected override UIView CreatePlatformElement()
    {
        CreateNavigationManager();

        ContentController = new SimpleShellSectionContentController
        {
            View = new SimpleContentView(),
        };

        var navigationController = new NativeSimpleShellSectionController(ContentController);

        ViewController = navigationController;
        AddToParentController(navigationController);

        return navigationController.View;
    }
}

#endif