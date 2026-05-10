using System;
#if ANDROID || IOS
using Shaunebu.MAUI.NFC;
#endif

namespace MauiApp4.Services
{
    public class NfcService : INfcService, IDisposable
    {
        public event EventHandler<NfcTagEventArgs>? TagDetected;

#if ANDROID || IOS
        public bool IsAvailable => CrossNFC.Current.IsAvailable;
        private bool _isListening;
        public bool IsListening => _isListening;
#else
        public bool IsAvailable => false;
        public bool IsListening => false;
#endif

        public NfcService()
        {
#if ANDROID || IOS
            CrossNFC.Current.OnMessageReceived += OnMessageReceived;
#endif
        }

        public void StartListening()
        {
#if ANDROID || IOS
            if (IsAvailable && !IsListening)
            {
                CrossNFC.Current.StartListening();
                _isListening = true;
            }
#endif
        }

        public void StopListening()
        {
#if ANDROID || IOS
            if (IsAvailable && IsListening)
            {
                CrossNFC.Current.StopListening();
                _isListening = false;
            }
#endif
        }

#if ANDROID || IOS
        private void OnMessageReceived(ITagInfo tagInfo)
        {
            if (tagInfo?.Identifier == null || tagInfo.Identifier.Length == 0)
                return;

            var args = new NfcTagEventArgs
            {
                Uid = BitConverter.ToString(tagInfo.Identifier).Replace("-", "")
            };

            TagDetected?.Invoke(this, args);
        }
#endif

        public void Dispose()
        {
#if ANDROID || IOS
            CrossNFC.Current.OnMessageReceived -= OnMessageReceived;
#endif
            StopListening();
        }
    }
}