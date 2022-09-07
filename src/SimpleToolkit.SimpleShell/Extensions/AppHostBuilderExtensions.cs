using SimpleToolkit.SimpleShell.Handlers;

namespace SimpleToolkit.SimpleShell
{
    public static class AppHostBuilderExtensions
    {
        /// <summary>
        /// Configures the SimpleToolkit.SimpleShell package.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static MauiAppBuilder UseSimpleShell(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(SimpleNavigationHost), typeof(SimpleNavigationHostHandler));
                handlers.AddHandler(typeof(SimpleShell), typeof(SimpleShellHandler));
                handlers.AddHandler(typeof(ShellItem), typeof(SimpleShellItemHandler));
                handlers.AddHandler(typeof(ShellSection), typeof(SimpleShellSectionHandler));
            });

            return builder;
        }
    }
}
