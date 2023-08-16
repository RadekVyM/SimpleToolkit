#if ANDROID

using Microsoft.Maui.Controls.Platform.Compatibility;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleShellSectionHandler
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

#endif