#if !(ANDROID || __IOS__ || MACCATALYST || WINDOWS)

using Microsoft.Maui.Handlers;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellHandler : ElementHandler<ISimpleShell, System.Object>
    {
        public SimpleShellHandler(IPropertyMapper mapper, CommandMapper commandMapper)
            : base(mapper, commandMapper)
        {
        }

        protected override System.Object CreatePlatformElement()
        {
            throw new NotImplementedException();
        }
    }
}

#endif