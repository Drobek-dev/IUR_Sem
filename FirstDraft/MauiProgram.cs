using FirstDraft.View;
using FirstDraft.ViewModel;
using Microsoft.Extensions.Logging;

namespace FirstDraft;

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

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<AllFestivalsPage>();
		builder.Services.AddSingleton<AllFestivalsPageVM>();
		return builder.Build();
	}
}
