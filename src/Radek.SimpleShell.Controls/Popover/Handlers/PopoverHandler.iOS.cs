#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using Radek.SimpleShell.Controls.Platform;

// Partially based on the .NET MAUI Community Toolkit Popup control - https://github.com/CommunityToolkit/Maui

namespace Radek.SimpleShell.Controls.Handlers
{
    public partial class PopoverHandler : ElementHandler<IPopover, UIPopoverViewController>
    {
        protected override UIPopoverViewController CreatePlatformElement()
        {
            return new UIPopoverViewController(MauiContext);
        }

        protected override void ConnectHandler(UIPopoverViewController platformView)
        {
            base.ConnectHandler(platformView);
            platformView.SetElement(VirtualView);
        }

        protected override void DisconnectHandler(UIPopoverViewController platformView)
        {
            base.DisconnectHandler(platformView);
            platformView.CleanUp();
        }

        private static void MapContent(PopoverHandler handler, IPopover popover)
        {
            handler.PlatformView.UpdateContent();
        }

        private static void MapShow(PopoverHandler handler, IPopover popover, object parentView)
        {
            if (parentView is not IElement anchor)
                return;

            handler.PlatformView?.InitializeView(popover, anchor);
        }

        private static async void MapHide(PopoverHandler handler, IPopover popover, object arg3)
        {
            var vc = handler.PlatformView.ViewController;
            if (vc is not null)
            {
                await vc.DismissViewControllerAsync(true);
            }
            //handler.PlatformView.CleanUp();
        }
    }
}

#endif