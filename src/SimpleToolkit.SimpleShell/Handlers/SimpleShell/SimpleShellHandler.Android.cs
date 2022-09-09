#if ANDROID

using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Handlers;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellHandler : ViewHandler<ISimpleShell, CustomFrameLayout>
    {
        protected override CustomFrameLayout CreatePlatformView()
        {
            var container = new CustomFrameLayout(MauiContext.Context)
            {
                Id = AView.GenerateViewId()
            };

            return container;
        }

        protected virtual AView GetNavigationHostContent()
        {
            return (navigationHost?.Handler as SimpleNavigationHostHandler)?.Container?.GetChildAt(0);
        }
    }
}

#endif