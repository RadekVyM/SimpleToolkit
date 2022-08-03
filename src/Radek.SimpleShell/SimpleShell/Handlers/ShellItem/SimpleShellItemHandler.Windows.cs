#if WINDOWS
// || (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)

using Microsoft.Maui.Handlers;
using WBorder = Microsoft.UI.Xaml.Controls.Border;

namespace Radek.SimpleShell.Handlers
{
    public partial class SimpleShellItemHandler : ElementHandler<ShellItem, WBorder>, IAppearanceObserver
    {
        protected override WBorder CreatePlatformElement()
        {
            shellSectionContainer = new WBorder();

            return shellSectionContainer;
        }
    }
}

#endif