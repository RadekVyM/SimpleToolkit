using Microsoft.Maui.Handlers;
using SimpleToolkit.Core.Platform;

namespace SimpleToolkit.Core.Handlers;

public partial class PopoverHandler : ElementHandler<IPopover, SimplePopupWindow>
{
    protected override SimplePopupWindow CreatePlatformElement()
    {
        return new SimplePopupWindow(
            MauiContext?.Context ?? throw new NullReferenceException("MauiContext should not be null here."),
            MauiContext);
    }

    protected override void ConnectHandler(SimplePopupWindow platformView)
    {
        platformView.SetElement(VirtualView);
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(SimplePopupWindow platformView)
    {
        platformView.CleanUp();
    }

    public static void MapContent(PopoverHandler handler, IPopover popover)
    {
        handler.PlatformView.SetContent(popover);
    }

    public static void MapIsAnimated(PopoverHandler handler, IPopover popover)
    {
        handler.PlatformView.IsAnimated = popover.IsAnimated;
    }

    public static void MapShow(PopoverHandler handler, IPopover popover, object? parentView)
    {
        if (parentView is not IElement anchor)
            return;
            
        handler.PlatformView.Show(anchor);
    }

    public static void MapHide(PopoverHandler handler, IPopover popover, object? arg3)
    {
        handler.PlatformView.Hide();
    }
}