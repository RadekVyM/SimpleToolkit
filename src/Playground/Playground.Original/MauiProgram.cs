using SimpleToolkit.Core;
using SimpleToolkit.SimpleShell;

namespace Playground.Original;

public static class MauiProgram
{
    internal const AppShellType UsedAppShell = AppShellType.Playground;

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>()
            .UseSimpleToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Font-Awesome-Solid.otf", "FontAwesomeSolid");
            });

        if (UsedAppShell == AppShellType.Playground)
            builder.DisplayContentBehindBars();

#if ANDROID && !ANDROID35_0_OR_GREATER
        builder.SetDefaultStatusBarAppearance(color: Colors.Black, lightElements: true);
        builder.SetDefaultNavigationBarAppearance(color: Colors.Black, lightElements: true);
#endif

#pragma warning disable CS8793 // The given expression always matches the provided pattern.
        if (UsedAppShell is not AppShellType.Normal)
        {
            builder.UseSimpleShell(true);
        }
#pragma warning restore CS8793 // The given expression always matches the provided pattern.

        return builder.Build();
    }
}