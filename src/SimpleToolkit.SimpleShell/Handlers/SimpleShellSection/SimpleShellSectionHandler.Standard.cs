#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleShellSectionHandler
{
    protected override System.Object CreatePlatformElement()
    {
        throw new NotImplementedException();
    }
}

#endif