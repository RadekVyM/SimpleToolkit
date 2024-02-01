#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using SimpleToolkit.SimpleShell.Platform;
using UIKit;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleShellHandler : ViewHandler<ISimpleShell, UIView>
{
    protected override UIView CreatePlatformView()
    {
        ViewController = new SimpleShellController
        {
            View = new SimpleContentView()
        };

        return ViewController.View;
    }

    protected virtual UIView GetNavigationHostContent()
    {
        return (navigationHost?.Handler as SimpleNavigationHostHandler)?.PlatformView?.Subviews.FirstOrDefault();
    }
}

#endif
