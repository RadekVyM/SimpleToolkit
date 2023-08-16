#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class NativeSimpleShellSectionHandler
{
    protected override System.Object CreatePlatformElement()
    {
        throw new NotImplementedException();
    }
}

#endif