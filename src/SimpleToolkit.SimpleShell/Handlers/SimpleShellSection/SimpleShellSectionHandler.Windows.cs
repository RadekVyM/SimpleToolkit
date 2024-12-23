using WGrid = Microsoft.UI.Xaml.Controls.Grid;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleShellSectionHandler
{
    protected override WGrid CreatePlatformElement()
    {
        CreateNavigationManager();
        return new WGrid();
    }
}