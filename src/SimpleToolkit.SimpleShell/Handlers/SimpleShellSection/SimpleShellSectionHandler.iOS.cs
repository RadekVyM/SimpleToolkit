#if IOS || MACCATALYST

using SimpleToolkit.SimpleShell.Extensions;
using SimpleToolkit.SimpleShell.Platform;
using UIKit;
using PlatformContentView = Microsoft.Maui.Platform.ContentView;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleShellSectionHandler
{
    public UIViewController ContentController { get; protected set; }
    public UIViewController ViewController { get; protected set; }

    protected override UIView CreatePlatformElement()
    {
        CreateNavigationManager();

        ContentController = new SimpleShellSectionContentController
        {
            View = new PlatformContentView(),
        };

        var navigationController = new SimpleShellSectionController(ContentController);

        navigationController.PopGestureRecognized += NavigationControllerPopGestureRecognized;

        ViewController = navigationController;
        AddToParentController(navigationController);

        return navigationController.View;
    }

    private void NavigationControllerPopGestureRecognized(object sender, EventArgs e)
    {
        var shell = VirtualView.FindParentOfType<SimpleShell>();

        if (shell is null)
            return;

        shell.SendBackButtonPressed();
    }
}

#endif