using Microsoft.Extensions.Logging;
using SimpleToolkit.Core;
using SimpleToolkit.SimpleShell;

namespace Sample.SimpleShell;

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
        builder.DisplayContentBehindBars();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}