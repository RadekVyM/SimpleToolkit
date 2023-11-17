#if ANDROID

using Microsoft.Maui.Controls.Platform.Compatibility;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class PlatformSimpleShellSectionHandler
{
    protected CustomFrameLayout RootContentContainer { get; private set; }

    protected override CustomFrameLayout CreatePlatformElement()
    {
        CreateNavigationManager();

        var root = new CustomFrameLayout(MauiContext.Context) { Id = AView.GenerateViewId() };
        RootContentContainer = new CustomFrameLayout(MauiContext.Context);

        return root;
    }
}

#endif