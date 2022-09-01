namespace SimpleToolkit.Core.Handlers
{
	public partial class PopoverHandler
    {
        public static IPropertyMapper<IPopover, PopoverHandler> Mapper = new PropertyMapper<IPopover, PopoverHandler>(ElementMapper)
        {
            [nameof(IPopover.Content)] = MapContent,
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

#if !(WINDOWS || ANDROID || IOS || MACCATALYST)

    public partial class PopoverHandler : Microsoft.Maui.Handlers.ElementHandler<IPopover, object>
    {
        protected override object CreatePlatformElement() => throw new NotSupportedException();

        public static void MapContent(PopoverHandler handler, IPopover popover)
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
}