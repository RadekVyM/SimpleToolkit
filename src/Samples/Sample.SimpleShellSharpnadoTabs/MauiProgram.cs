using Microsoft.Extensions.Logging;
using Sharpnado.Tabs;
using SimpleToolkit.SimpleButton;
using SimpleToolkit.SimpleShell;

namespace Sample.SimpleShellSharpnadoTabs;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSharpnadoTabs(false)
            .UseSimpleShell()
            .UseSimpleButton()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FontAwesome");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}