using Microsoft.Extensions.Logging;
using SimpleToolkit.SimpleShell;

namespace SimpleToolkit.SimpleShellSample;

public static class MauiProgram
{
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

		builder.UseSimpleShell();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

