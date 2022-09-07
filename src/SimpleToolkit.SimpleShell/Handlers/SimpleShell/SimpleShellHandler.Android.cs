#if ANDROID

using Microsoft.Maui.Handlers;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellHandler : ViewHandler<ISimpleShell, AView>
    {
        protected virtual AView GetNavigationHostContent()
        {
            return (navigationHost?.Handler as SimpleNavigationHostHandler)?.Container?.GetChildAt(0);
        }
    }
}

#endif