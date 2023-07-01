#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using SimpleToolkit.SimpleShell.Platform;
using UIKit;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellSectionHandler : ElementHandler<ShellSection, UIView>
    {
        protected override UIView CreatePlatformElement()
        {
            CreateNavigationManager();
            var container = new SimpleContentView();
            return container;
        }
    }
}

#endif