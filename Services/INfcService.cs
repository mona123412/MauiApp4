using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace MauiApp4.Services
{
    public interface INfcService
    {
        bool IsAvailable { get; }
        bool IsListening { get; }
        void StartListening();
        void StopListening();
        event EventHandler<NfcTagEventArgs>? TagDetected;
    }

    public class NfcTagEventArgs : EventArgs
    {
        public string? Uid { get; set; }
    }
}
