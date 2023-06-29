#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Platform;
using UIKit;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellItemHandler : ElementHandler<ShellItem, UIView>
    {
        protected override UIView CreatePlatformElement()
        {
            shellSectionContainer = new SimpleContentView();
            return shellSectionContainer;
        }

        private void UpdateShellSectionContainerContent()
        {
            if (currentShellSectionHandler.PlatformView != (UIKit.UIView)shellSectionContainer.Subviews.FirstOrDefault())
            {
                shellSectionContainer.ClearSubviews();
                shellSectionContainer.AddSubview(currentShellSectionHandler.PlatformView);
            }
        }
    }
}

#endif