using MauiApp4.Services;
using Microsoft.Extensions.Logging;
#if ANDROID
using AndroidNfc = MauiApp4.Platforms.Android.NfcService; // alias per evitare ambiguità
#endif

namespace MauiApp4
{
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
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<GameState>();

#if ANDROID
            builder.Services.AddSingleton<INfcService, AndroidNfc>();
#else
            builder.Services.AddSingleton<INfcService, NfcService>();
#endif

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}