#if ANDROID

using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Handlers;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellItemHandler : ElementHandler<ShellItem, CustomFrameLayout>, IAppearanceObserver
    {
        protected override CustomFrameLayout CreatePlatformElement()
        {
            shellSectionContainer = new CustomFrameLayout(MauiContext.Context)
            {
                Id = AView.GenerateViewId()
            };

            return shellSectionContainer;
        }
    }
}

#endif