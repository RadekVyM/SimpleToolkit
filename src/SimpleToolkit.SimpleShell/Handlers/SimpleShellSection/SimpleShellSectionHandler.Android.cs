using Android.Widget;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleShellSectionHandler
{
    protected override FrameLayout CreatePlatformElement()
    {
        CreateNavigationManager();

        return new FrameLayout(MauiContext?.Context ?? throw new NullReferenceException("MauiContext should not be null here."))
        {
            Id = AView.GenerateViewId()
        };
    }
}