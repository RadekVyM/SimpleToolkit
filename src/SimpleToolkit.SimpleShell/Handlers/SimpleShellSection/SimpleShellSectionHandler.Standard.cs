#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

using Microsoft.Maui.Handlers;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellSectionHandler : ElementHandler<ShellSection, System.Object>
    {
        protected override System.Object CreatePlatformElement()
        {
            throw new NotImplementedException();
        }
    }
}

#endif