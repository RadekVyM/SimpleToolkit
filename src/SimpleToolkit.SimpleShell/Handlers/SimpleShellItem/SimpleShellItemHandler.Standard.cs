#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

using Microsoft.Maui.Handlers;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleShellItemHandler : ElementHandler<ShellItem, System.Object>
{
    protected override System.Object CreatePlatformElement()
    {
        throw new NotImplementedException();
    }

    private void UpdatePlatformViewContent()
    {
    }
}

#endif