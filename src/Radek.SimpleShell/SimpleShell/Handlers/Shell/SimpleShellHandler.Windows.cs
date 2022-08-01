#if WINDOWS

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using WFrameworkElement = Microsoft.UI.Xaml.FrameworkElement;

namespace Radek.SimpleShell.Handlers
{
    public partial class SimpleShellHandler : ViewHandler<ISimpleShell, WFrameworkElement>
    {
        protected virtual WFrameworkElement GetNavigationHostContent()
        {
            return (navigationHost?.Handler as SimpleNavigationHostHandler)?.Container?.Child as WFrameworkElement;
        }
    }
}

#endif
