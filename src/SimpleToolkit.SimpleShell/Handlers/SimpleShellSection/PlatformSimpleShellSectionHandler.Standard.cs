﻿#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class PlatformSimpleShellSectionHandler
{
    protected System.Object RootContentContainer => throw new NotImplementedException();

    protected override System.Object CreatePlatformElement()
    {
        throw new NotImplementedException();
    }
}

#endif