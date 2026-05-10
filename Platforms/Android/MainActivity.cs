using Android.App;
using Android.Content;
using Android.OS;
using MauiApp4.Services;
using MauiApp4.Platforms.Android; // il tuo NfcService Android

namespace MauiApp4;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnResume()
    {
        base.OnResume();
        var nfcService = IPlatformApplication.Current?.Services
            .GetService<INfcService>() as Platforms.Android.NfcService;
        nfcService?.Init(this);
        nfcService?.StartListening();
    }

    protected override void OnPause()
    {
        base.OnPause();
        var nfcService = IPlatformApplication.Current?.Services
            .GetService<INfcService>() as Platforms.Android.NfcService;
        nfcService?.StopListening();
    }

    protected override void OnNewIntent(Intent intent)
    {
        base.OnNewIntent(intent);
        var nfcService = IPlatformApplication.Current?.Services
            .GetService<INfcService>() as Platforms.Android.NfcService;
        nfcService?.HandleIntent(intent);
    }
}