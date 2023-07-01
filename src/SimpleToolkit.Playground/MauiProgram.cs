using SimpleToolkit.Core;

namespace SimpleToolkit.SimpleShell.Playground
{
    public static class MauiProgram
    {
        internal const AppShellType UsedAppShell = AppShellType.Playground;

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Font-Awesome-Solid.otf", "FontAwesomeSolid");
                });

            builder.UseSimpleToolkit();

            builder.DisplayContentBehindBars();

#if ANDROID
            builder.SetDefaultStatusBarAppearance(color: Colors.Transparent, lightElements: false);
            builder.SetDefaultNavigationBarAppearance(color: Colors.Transparent, lightElements: false);
#endif

            if (UsedAppShell is not AppShellType.Normal)
            {
                builder.UseSimpleShell();
            }

            return builder.Build();
        }
    }
}