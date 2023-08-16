#if IOS || MACCATALYST

using UIKit;

namespace SimpleToolkit.SimpleShell.Platform;

public class NativeSimpleShellSectionController : UINavigationController
{
    public NativeSimpleShellSectionController(UIViewController rootViewController) : base(rootViewController)
    {
        this.NavigationBarHidden = true;
    }
}

#endif