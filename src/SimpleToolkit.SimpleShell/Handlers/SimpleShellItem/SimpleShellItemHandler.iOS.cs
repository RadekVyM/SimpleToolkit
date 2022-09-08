#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using UIKit;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellItemHandler : ElementHandler<ShellItem, UIView>
    {
        protected override UIView CreatePlatformElement()
        {
            shellSectionContainer = new CustomContentView();
            return shellSectionContainer;
        }
    }
}

#endif