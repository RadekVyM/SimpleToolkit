namespace Radek.SimpleShell.Controls.Handlers
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
    // || IOS || MACCATALYST
#if !(WINDOWS || ANDROID)

    public partial class PopoverHandler : Microsoft.Maui.Handlers.ElementHandler<IPopover, object>
    {
        protected override object CreatePlatformElement() => throw new NotSupportedException();

        private static void MapContent(PopoverHandler handler, IPopover popover)
        {
        }

        private static void MapHide(PopoverHandler handler, IPopover popover, object arg3)
        {
        }

        private static void MapShow(PopoverHandler handler, IPopover popover, object parentView)
        {
        }
    }

#endif
}