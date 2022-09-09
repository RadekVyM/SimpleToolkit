#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using UIKit;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellHandler : ViewHandler<ISimpleShell, UIView>
    {
        protected override UIView CreatePlatformView()
        {
            var container = new CustomContentView();
            return container;
        }

        protected virtual UIView GetNavigationHostContent()
        {
            return (navigationHost?.Handler as SimpleNavigationHostHandler)?.Container?.Subviews.FirstOrDefault();
        }
    }
}

#endif
