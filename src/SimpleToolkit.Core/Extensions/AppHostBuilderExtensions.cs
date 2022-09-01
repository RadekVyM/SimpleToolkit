using SimpleToolkit.Core.Handlers;

namespace SimpleToolkit.Core
{
    public static class AppHostBuilderExtensions
    {
        public static MauiAppBuilder UseSimpleToolkit(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(Icon), typeof(IconHandler));
                handlers.AddHandler(typeof(Popover), typeof(PopoverHandler));
            });

            return builder;
        }
    }
}
