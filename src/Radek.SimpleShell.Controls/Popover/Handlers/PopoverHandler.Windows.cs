#if WINDOWS

using Microsoft.Maui.Handlers;
using Radek.SimpleShell.Controls.Platform;

namespace Radek.SimpleShell.Controls.Handlers
{
    public partial class PopoverHandler : ElementHandler<IPopover, SimpleFlyout>
    {
        protected override SimpleFlyout CreatePlatformElement()
        {
            return new SimpleFlyout(MauiContext);
        }

        protected override void ConnectHandler(SimpleFlyout platformView)
        {
            platformView.SetElement(VirtualView);
            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(SimpleFlyout platformView)
        {
            platformView.CleanUp();
        }

        private static void MapContent(PopoverHandler handler, IPopover popover)
        {
            handler.PlatformView.ConfigureControl();
        }

        private static void MapShow(PopoverHandler handler, IPopover popover, object parentView)
        {
            if (parentView is IElement anchor)
            {
                handler.PlatformView.Show(anchor);
            }
        }

        private static void MapHide(PopoverHandler handler, IPopover popover, object arg3)
        {
            handler.PlatformView.Hide();
        }
    }
}

#endif