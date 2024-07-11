using Android.Widget;
using Microsoft.Maui.Handlers;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleShellItemHandler : ElementHandler<ShellItem, FrameLayout>
{
    protected override FrameLayout CreatePlatformElement()
    {
        return new FrameLayout(MauiContext.Context)
        {
            Id = AView.GenerateViewId()
        };
    }

    private void UpdatePlatformViewContent()
    {
        if (currentShellSectionHandler.PlatformView != PlatformView.GetChildAt(0))
        {
            PlatformView.RemoveAllViews();
            if (currentShellSectionHandler.PlatformView is not null)
                PlatformView.AddView(currentShellSectionHandler.PlatformView);
        }
    }
}