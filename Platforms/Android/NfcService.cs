// Platforms/Android/NfcService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Nfc;
using Android.Nfc.Tech;
using MauiApp4.Services; // per INfcService e NfcTagEventArgs

namespace MauiApp4.Platforms.Android
{
    public class NfcService : INfcService
    {
        private NfcAdapter? _adapter;
        private Activity? _activity;

        public event EventHandler<NfcTagEventArgs>? TagDetected;
        public bool IsAvailable => _adapter?.IsEnabled == true;
        public bool IsListening { get; private set; }

        public void Init(Activity activity)
        {
            _activity = activity;
            _adapter = NfcAdapter.GetDefaultAdapter(activity);
        }

        public void StartListening()
        {
            if (_activity == null || _adapter == null) return;

            var intent = new global::Android.Content.Intent(_activity, _activity.GetType())
                .AddFlags(global::Android.Content.ActivityFlags.SingleTop);

            var pendingIntent = global::Android.App.PendingIntent.GetActivity(
                _activity, 0, intent,
                global::Android.App.PendingIntentFlags.Mutable);

            _adapter.EnableForegroundDispatch(_activity, pendingIntent, null, null);
            IsListening = true;
        }

        public void StopListening()
        {
            if (_activity != null)
                _adapter?.DisableForegroundDispatch(_activity);
            IsListening = false;
        }

        public void HandleIntent(global::Android.Content.Intent intent)
        {
            if (intent?.Action != NfcAdapter.ActionTechDiscovered &&
                intent?.Action != NfcAdapter.ActionTagDiscovered) return;

            var tag = intent.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;
            if (tag == null) return;

            var uid = BitConverter.ToString(tag.GetId()).Replace("-", "");
            TagDetected?.Invoke(this, new NfcTagEventArgs { Uid = uid });
        }
    }
}