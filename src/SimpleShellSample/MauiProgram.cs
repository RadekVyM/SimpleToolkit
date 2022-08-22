using Radek.SimpleShell;
using Radek.SimpleShell.Controls;

namespace SimpleShellSample;

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
            });
        
        builder.ConfigureSimpleShellControls();

        if (UseSimpleShell)
        {
            builder.AddSimpleShellHandlers();
        }

        return builder.Build();
	}
}
