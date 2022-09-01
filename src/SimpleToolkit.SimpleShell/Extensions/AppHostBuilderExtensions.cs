using SimpleToolkit.SimpleShell.Handlers;

namespace SimpleToolkit.SimpleShell
{
    public static class AppHostBuilderExtensions
    {
        public static MauiAppBuilder ConfigureSimpleShell(this MauiAppBuilder builder)
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
