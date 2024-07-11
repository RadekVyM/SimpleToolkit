using Microsoft.Maui.Handlers;
using SimpleToolkit.Core.Platform;

// Partially based on the .NET MAUI Community Toolkit Popup control - https://github.com/CommunityToolkit/Maui

namespace SimpleToolkit.Core.Handlers;

public partial class PopoverHandler : ElementHandler<IPopover, PopoverViewController>
{
    protected override PopoverViewController CreatePlatformElement()
    {
        return new PopoverViewController(MauiContext);
    }

    protected override void ConnectHandler(PopoverViewController platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SetElement(VirtualView);
    }

    protected override void DisconnectHandler(PopoverViewController platformView)
    {
        base.DisconnectHandler(platformView);
        platformView.CleanUp();
    }

    public static void MapContent(PopoverHandler handler, IPopover popover)
    {
        handler.PlatformView.UpdateContent();
    }

    public static void MapIsAnimated(PopoverHandler handler, IPopover popover)
    {
        handler.PlatformView.IsAnimated = popover.IsAnimated;
    }

    public static void MapPermittedArrowDirections(PopoverHandler handler, IPopover popover)
    {
        handler.PlatformView.PermittedArrowDirections = popover.PermittedArrowDirections.ToUIPopoverArrowDirection();
    }

    public static async void MapShow(PopoverHandler handler, IPopover popover, object parentView)
    {
        if (parentView is not IElement anchor)
            return;

        try { await handler.PlatformView?.Show(popover, anchor); }
        catch { throw; }
    }

    public static async void MapHide(PopoverHandler handler, IPopover popover, object arg3)
    {
        var vc = handler.PlatformView.ViewController;
        if (vc is not null)
        {
            try { await vc.DismissViewControllerAsync(true); }
            catch { throw; }
        }
        //handler.PlatformView.CleanUp();
    }
}