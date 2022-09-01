#if ANDROID

using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Handlers;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellSectionHandler : ElementHandler<ShellSection, CustomFrameLayout>, IAppearanceObserver
    {
        protected override CustomFrameLayout CreatePlatformElement()
        {
            CreateNavigationManager();
            var container = new CustomFrameLayout(MauiContext.Context)
            {
                Id = AView.GenerateViewId()
            };

            return container;
        }
    }
}

#endif