#if __IOS__ || MACCATALYST

using Microsoft.Maui.Handlers;
using UIKit;

namespace Radek.SimpleShell.Handlers
{
    public partial class SimpleShellItemHandler : ElementHandler<ShellItem, UIView>, IAppearanceObserver
    {
        protected override UIView CreatePlatformElement()
        {
            var container = new UIView();
            return container;
        }
    }
}

#endif