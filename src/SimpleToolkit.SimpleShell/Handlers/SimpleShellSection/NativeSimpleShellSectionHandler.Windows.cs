#if WINDOWS

using WGrid = Microsoft.UI.Xaml.Controls.Grid;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class NativeSimpleShellSectionHandler
{
    protected WGrid RootContentContainer => null;
}

#endif