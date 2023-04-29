#if WINDOWS

using Microsoft.Maui.Handlers;
using WBorder = Microsoft.UI.Xaml.Controls.Border;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellItemHandler : ElementHandler<ShellItem, WBorder>
    {
        protected override WBorder CreatePlatformElement()
        {
            shellSectionContainer = new WBorder();
            return shellSectionContainer;
        }

        private void UpdateShellSectionContainerContent()
        {
            if (currentShellSectionHandler.PlatformView != (Microsoft.UI.Xaml.Controls.Grid)shellSectionContainer.Child)
                shellSectionContainer.Child = currentShellSectionHandler.PlatformView;
        }
    }
}

#endif