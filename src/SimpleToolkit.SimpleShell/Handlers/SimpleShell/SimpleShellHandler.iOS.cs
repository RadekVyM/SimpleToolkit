#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using SimpleToolkit.SimpleShell.Handlers.Platform;
using UIKit;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellHandler : ViewHandler<ISimpleShell, UIView>
    {
        private SimpleNavigationController navigationController;

        protected override UIView CreatePlatformView()
        {
            var contentController = new UIViewController
            {
                View = new CustomContentView(),
            };

            navigationController = new SimpleNavigationController();

            navigationController.SetViewControllers(new[] { contentController }, false);

            navigationController.PopGestureRecognized += NavigationControllerPopGestureRecognized;

            return navigationController.View;
        }

        protected virtual UIView GetNavigationHostContent()
        {
            return (navigationHost?.Handler as SimpleNavigationHostHandler)?.Container?.Subviews.FirstOrDefault();
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
