#if ANDROID

using Microsoft.Maui.Controls.Platform.Compatibility;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class NativeSimpleShellSectionHandler
{
    protected CustomFrameLayout RootContentContainer => null;

    protected override CustomFrameLayout CreatePlatformElement()
    {
        throw new NotImplementedException();
    }
}

#endif