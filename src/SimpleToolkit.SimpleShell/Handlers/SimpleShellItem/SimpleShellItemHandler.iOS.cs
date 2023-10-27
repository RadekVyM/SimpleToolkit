#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;
using PlatformContentView = Microsoft.Maui.Platform.ContentView;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleShellItemHandler : ElementHandler<ShellItem, UIView>
{
    protected override UIView CreatePlatformElement()
    {
        return new PlatformContentView();
    }

    private void UpdatePlatformViewContent()
    {
        if (currentShellSectionHandler.PlatformView != PlatformView.Subviews.FirstOrDefault())
        {
            PlatformView.ClearSubviews();
            PlatformView.AddSubview(currentShellSectionHandler.PlatformView);
        }
    }
}

#endif