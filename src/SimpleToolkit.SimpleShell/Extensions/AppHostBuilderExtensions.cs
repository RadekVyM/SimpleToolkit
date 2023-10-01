using SimpleToolkit.SimpleShell.Handlers;

namespace SimpleToolkit.SimpleShell;

public static class AppHostBuilderExtensions
{
    /// <summary>
    /// Configures the SimpleToolkit.SimpleShell package.
    /// </summary>
    /// <param name="builder">Instance of <see cref="MauiAppBuilder"/>.</param>
    /// <param name="usePlatformTransitions">Indicates whether to use platform specific page transitions or not. The default option is <see cref="true"/>.</param>
    /// <returns>Instance of <see cref="MauiAppBuilder"/>.</returns>
    public static MauiAppBuilder UseSimpleShell(this MauiAppBuilder builder, bool usePlatformTransitions = true)
    {
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler(typeof(SimpleNavigationHost), typeof(SimpleNavigationHostHandler));
            handlers.AddHandler(typeof(SimpleShell), typeof(SimpleShellHandler));
            handlers.AddHandler(typeof(ShellItem), typeof(SimpleShellItemHandler));
            if (usePlatformTransitions)
                handlers.AddHandler(typeof(ShellSection), typeof(PlatformSimpleShellSectionHandler));
            else
                handlers.AddHandler(typeof(ShellSection), typeof(SimpleShellSectionHandler));
        });

        SimpleShell.UsesPlatformTransitions = usePlatformTransitions;

        return builder;
    }
}