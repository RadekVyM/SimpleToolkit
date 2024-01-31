#if ANDROID

using Android.Widget;
using Microsoft.Maui.Controls.Platform.Compatibility;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class PlatformSimpleShellSectionHandler
{
    protected FrameLayout RootContentContainer { get; private set; }

    protected override FrameLayout CreatePlatformElement()
    {
        CreateNavigationManager();

        var root = new FrameLayout(MauiContext.Context) { Id = AView.GenerateViewId() };
        RootContentContainer = new FrameLayout(MauiContext.Context);

        return root;
    }
}

#endif