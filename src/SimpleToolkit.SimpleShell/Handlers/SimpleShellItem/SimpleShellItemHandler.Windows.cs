using Microsoft.Maui.Handlers;
using WBorder = Microsoft.UI.Xaml.Controls.Border;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleShellItemHandler : ElementHandler<ShellItem, WBorder>
{
    protected override WBorder CreatePlatformElement()
    {
        return new WBorder();
    }

    private void UpdatePlatformViewContent()
    {
        if (currentShellSectionHandler.PlatformView != PlatformView.Child)
            PlatformView.Child = currentShellSectionHandler.PlatformView;
    }
}