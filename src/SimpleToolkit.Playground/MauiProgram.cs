using SimpleToolkit.Core;
using CommunityToolkit.Maui;
#if IOS || MACCATALYST
using CommunityToolkit.Maui.Views;
#endif

namespace SimpleToolkit.SimpleShell.Playground
{
    public static class MauiProgram
    {
        internal const AppShellType UsedAppShell = AppShellType.NoTabs;

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkitMediaElement()
                .UseSimpleToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Font-Awesome-Solid.otf", "FontAwesomeSolid");
                });

#if IOS || MACCATALYST

            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<MediaElement, CustomMediaElementHandler>();
            });

#endif

            builder.DisplayContentBehindBars();

#if ANDROID
            builder.SetDefaultStatusBarAppearance(color: Colors.Transparent, lightElements: false);
            builder.SetDefaultNavigationBarAppearance(color: Colors.Transparent, lightElements: false);
#endif
            if (UsedAppShell is not AppShellType.Normal)
            {
                builder.UseSimpleShell(false);
            }

            return builder.Build();
        }
    }
}