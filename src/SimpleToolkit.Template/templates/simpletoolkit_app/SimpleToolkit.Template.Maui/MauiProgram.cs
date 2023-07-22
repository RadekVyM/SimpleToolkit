using Microsoft.Extensions.Logging;
using SimpleToolkit.Core;
using SimpleToolkit.SimpleShell;
using SimpleToolkit.Template.Core.Interfaces.Services;
using SimpleToolkit.Template.Core.Interfaces.ViewModels;
using SimpleToolkit.Template.Core.ViewModels;
using SimpleToolkit.Template.Maui.Services;
using SimpleToolkit.Template.Maui.Views.Pages;

namespace SimpleToolkit.Template.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSimpleToolkit()
            .UseSimpleShell()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        //-:cnd:noEmit

#if DEBUG
        builder.Logging.AddDebug();
#endif

#if ANDROID || IOS
        builder.DisplayContentBehindBars();
#endif
#if ANDROID
        builder.SetDefaultStatusBarAppearance(Colors.Transparent, false);
#endif
        //+:cnd:noEmit

        builder.Services.AddTransient<AppShell>();

        builder.Services.AddTransient<MainPage>();

        builder.Services.AddTransient<IMainPageViewModel, MainPageViewModel>();

        builder.Services.AddSingleton<INavigationService, NavigationService>();

        return builder.Build();
    }
}