#if WINDOWS

using Microsoft.Maui.Handlers;
using WGrid = Microsoft.UI.Xaml.Controls.Grid;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellSectionHandler : ElementHandler<ShellSection, WGrid>
    {
        protected override WGrid CreatePlatformElement()
        {
            navigationManager = CreateNavigationManager();
            return new WGrid();
        }
    }
}

#endif