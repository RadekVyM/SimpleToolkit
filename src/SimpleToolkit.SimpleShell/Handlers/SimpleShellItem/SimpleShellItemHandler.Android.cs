#if ANDROID

using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Handlers;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellItemHandler : ElementHandler<ShellItem, CustomFrameLayout>
    {
        protected override CustomFrameLayout CreatePlatformElement()
        {
            shellSectionContainer = new CustomFrameLayout(MauiContext.Context)
            {
                Id = AView.GenerateViewId()
            };

            return shellSectionContainer;
        }

        private void UpdateShellSectionContainerContent()
        {
            if (currentShellSectionHandler.PlatformView != shellSectionContainer.GetChildAt(0))
            {
                shellSectionContainer.RemoveAllViews();
                if (currentShellSectionHandler.PlatformView is not null)
                    shellSectionContainer.AddView(currentShellSectionHandler.PlatformView);
            }
        }
    }
}

#endif