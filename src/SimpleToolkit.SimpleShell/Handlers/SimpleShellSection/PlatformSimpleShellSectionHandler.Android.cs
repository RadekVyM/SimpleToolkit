using Android.Widget;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class PlatformSimpleShellSectionHandler
{
    protected FrameLayout RootContentContainer { get; private set; } = null!;

    protected override FrameLayout CreatePlatformElement()
    {
        CreateNavigationManager();

        var context = MauiContext?.Context ?? throw new NullReferenceException("MauiContext should not be null here.");
        var root = new FrameLayout(context) { Id = AView.GenerateViewId() };
        RootContentContainer = new FrameLayout(context);

        return root;
    }
}