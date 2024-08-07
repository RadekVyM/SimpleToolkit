﻿using SimpleToolkit.Core;
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

#if ANDROID
        builder.SetDefaultStatusBarAppearance(color: Colors.Transparent, lightElements: false);
        builder.SetDefaultNavigationBarAppearance(color: Colors.Transparent, lightElements: false);
#endif
        if (UsedAppShell is not AppShellType.Normal)
        {
            builder.UseSimpleShell(true);
        }

        return builder.Build();
    }
}