#if IOS || MACCATALYST

using SimpleToolkit.SimpleShell.Extensions;
using SimpleToolkit.SimpleShell.Platform;
using UIKit;
using PlatformContentView = Microsoft.Maui.Platform.ContentView;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class PlatformSimpleShellSectionHandler
{
    public UIViewController RootContentController { get; private set; }
    public UINavigationController ViewController { get; private set; }
    protected UIView RootContentContainer => RootContentController.View;

    protected override UIView CreatePlatformElement()
    {
        CreateNavigationManager();

        RootContentController = new SimpleShellSectionContentController
        {
            View = new PlatformContentView(),
        };

        var shell = VirtualView.FindParentOfType<SimpleShell>();
        var navigationController = new PlatformSimpleShellSectionController(RootContentController, shell);

        ViewController = navigationController;
        AddToParentController(navigationController);

        return navigationController.View;
    }
}

#endif