#if !ANDROID && !(__IOS__ || MACCATALYST) && !WINDOWS

using Microsoft.Maui.Handlers;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleNavigationHostHandler : ElementHandler<ISimpleNavigationHost, System.Object>
    {
        public SimpleNavigationHostHandler(IPropertyMapper mapper, CommandMapper commandMapper)
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