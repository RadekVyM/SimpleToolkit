#if WINDOWS

using Microsoft.Maui.Handlers;
using WFrameworkElement = Microsoft.UI.Xaml.FrameworkElement;
using WBorder = Microsoft.UI.Xaml.Controls.Border;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellHandler : ViewHandler<ISimpleShell, WBorder>
    {
        protected override WBorder CreatePlatformView()
        {
            var container = new WBorder();
            return container;
        }

        protected virtual WFrameworkElement GetNavigationHostContent()
        {
            return (navigationHost?.Handler as SimpleNavigationHostHandler)?.PlatformView?.Children.FirstOrDefault() as WFrameworkElement;
        }
    }
}

#endif
