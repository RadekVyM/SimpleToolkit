#if WINDOWS

using Microsoft.Maui.Handlers;
using WFrame = Microsoft.UI.Xaml.Controls.Frame;

namespace Radek.SimpleShell.Handlers
{
    public partial class SimpleShellSectionHandler : ElementHandler<ShellSection, WFrame>, IAppearanceObserver
    {
        protected override WFrame CreatePlatformElement()
        {
            navigationManager = CreateNavigationManager();
            return new WFrame();
        }
    }
}

#endif