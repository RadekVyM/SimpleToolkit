namespace SimpleToolkit.Core.Handlers;

public partial class PopoverHandler
{
    public static IPropertyMapper<IPopover, PopoverHandler> Mapper = new PropertyMapper<IPopover, PopoverHandler>(ElementMapper)
    {
        [nameof(IPopover.Content)] = MapContent,
        [nameof(IPopover.IsAnimated)] = MapIsAnimated,
#if IOS || MACCATALYST
        [nameof(IPopover.PermittedArrowDirections)] = MapPermittedArrowDirections,
#endif
    };

    public static CommandMapper<IPopover, PopoverHandler> CommandMapper = new(ElementCommandMapper)
    {
        [nameof(IPopover.Show)] = MapShow,
        [nameof(IPopover.Hide)] = MapHide,
    };


    public PopoverHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
    {
    }

    public PopoverHandler()
        : base(Mapper, CommandMapper)
    {
    }
}