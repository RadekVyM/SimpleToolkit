using SimpleToolkit.Core.Handlers;

namespace SimpleToolkit.Core;

public static class AppHostBuilderExtensions
{
    /// <summary>
    /// Configures the SimpleToolkit.Core package.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static MauiAppBuilder UseSimpleToolkit(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler(typeof(Icon), typeof(IconHandler));
            handlers.AddHandler(typeof(Popover), typeof(PopoverHandler));
            handlers.AddHandler(typeof(ContentButton), typeof(ContentButtonHandler));
        });

        return builder;
    }
}