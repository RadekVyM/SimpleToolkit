#if WINDOWS

using WGrid = Microsoft.UI.Xaml.Controls.Grid;
using WFrame = Microsoft.UI.Xaml.Controls.Frame;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class PlatformSimpleShellSectionHandler
{
    protected WGrid RootContentContainer { get; private set; }

    protected override WFrame CreatePlatformElement()
    {
        CreateNavigationManager();

        var root = new WFrame { IsNavigationStackEnabled = true };
        RootContentContainer = new WGrid();

        return root;
    }
}

#endif