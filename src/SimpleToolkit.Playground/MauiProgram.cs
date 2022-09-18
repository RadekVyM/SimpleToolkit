using SimpleToolkit.Core;

namespace SimpleToolkit.SimpleShell.Playground
{
    public static class MauiProgram
    {
        public const bool UseSimpleShell = true;

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
            builder.SetDefaultStatusBarAppearance(color: Colors.DarkOrange, lightElements: true);
            builder.SetDefaultNavigationBarAppearance(color: Colors.White, lightElements: false);
#endif

            if (UseSimpleShell)
            {
                builder.UseSimpleShell();
            }

            return builder.Build();
        }
    }
}