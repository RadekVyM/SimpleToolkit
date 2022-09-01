#if __IOS__ || MACCATALYST

using Microsoft.Maui.Handlers;
using UIKit;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellSectionHandler : ElementHandler<ShellSection, UIView>, IAppearanceObserver
    {
        protected override UIView CreatePlatformElement()
        {
            CreateNavigationManager();

            var container = new CustomContentView();
            return container;
        }
    }
}

#endif