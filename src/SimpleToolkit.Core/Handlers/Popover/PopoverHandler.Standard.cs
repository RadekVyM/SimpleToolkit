#if !(WINDOWS || ANDROID || IOS || MACCATALYST)

namespace SimpleToolkit.Core.Handlers;

public partial class PopoverHandler : Microsoft.Maui.Handlers.ElementHandler<IPopover, object>
{
    protected override object CreatePlatformElement() => throw new NotSupportedException();

    public static void MapContent(PopoverHandler handler, IPopover popover)
    {
    }

    public static void MapIsAnimated(PopoverHandler handler, IPopover popover)
    {
    }

    public static void MapHide(PopoverHandler handler, IPopover popover, object arg3)
    {
    }

    public static void MapShow(PopoverHandler handler, IPopover popover, object parentView)
    {
    }
}

#endif