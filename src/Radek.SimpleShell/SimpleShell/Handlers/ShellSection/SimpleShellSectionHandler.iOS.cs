#if __IOS__ || MACCATALYST

using Microsoft.Maui.Handlers;
using UIKit;

namespace Radek.SimpleShell.Handlers
{
    public partial class SimpleShellSectionHandler : ElementHandler<ShellSection, UIView>, IAppearanceObserver
    {
        protected override UIView CreatePlatformElement()
        {
            CreateNavigationManager();

            var container = new UIView();
            return container;
        }
    }
}

#endif