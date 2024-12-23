using Microsoft.Maui.Handlers;
using SimpleToolkit.Core.Platform;

// Partially based on the .NET MAUI Community Toolkit Popup control - https://github.com/CommunityToolkit/Maui

namespace SimpleToolkit.Core.Handlers;

public partial class PopoverHandler : ElementHandler<IPopover, SimpleFlyout>
{
    protected override SimpleFlyout CreatePlatformElement()
    {
        return new SimpleFlyout(MauiContext ?? throw new NullReferenceException("MauiContext should not be null here."));
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

    public static void MapContent(PopoverHandler handler, IPopover popover)
    {
        handler.PlatformView.UpdateContent();
    }

    public static void MapIsAnimated(PopoverHandler handler, IPopover popover)
    {
        handler.PlatformView.AreOpenCloseAnimationsEnabled = popover.IsAnimated;
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