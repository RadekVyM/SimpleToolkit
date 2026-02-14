using SimpleToolkit.SimpleButton.Handlers;

namespace SimpleToolkit.SimpleButton;

public static class AppHostBuilderExtensions
{
    /// <summary>
    /// Configures the SimpleToolkit.SimpleButton package.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static MauiAppBuilder UseSimpleButton(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler<SimpleButton, SimpleButtonHandler>();
        });

        return builder;
    }
}