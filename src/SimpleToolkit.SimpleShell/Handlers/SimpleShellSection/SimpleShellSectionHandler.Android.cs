using Android.Widget;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleShellSectionHandler
{
    protected override FrameLayout CreatePlatformElement()
    {
        CreateNavigationManager();

        return new FrameLayout(MauiContext.Context)
        {
            Id = AView.GenerateViewId()
        };
    }
}