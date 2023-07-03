#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using SimpleToolkit.SimpleShell.Platform;
using UIKit;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellHandler : ViewHandler<ISimpleShell, UIView>
    {
        public UIViewController ContentController { get; private set; }

        protected override UIView CreatePlatformView()
        {
            ContentController = new UIViewController
            {
                View = new SimpleContentView(),
            };

            SimpleNavigationController navigationController = new SimpleNavigationController(ContentController);

            navigationController.PopGestureRecognized += NavigationControllerPopGestureRecognized;

            ViewController = navigationController;

            return navigationController.View;
        }

        protected virtual UIView GetNavigationHostContent()
        {
            return (navigationHost?.Handler as SimpleNavigationHostHandler)?.PlatformView?.Subviews.FirstOrDefault();
        }

        private void NavigationControllerPopGestureRecognized(object sender, EventArgs e)
        {
            if (VirtualView is not SimpleShell shell)
                return;

            shell.SendBackButtonPressed();
        }
    }
}

#endif
