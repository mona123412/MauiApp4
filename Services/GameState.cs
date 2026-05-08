using System;
using System.Timers;

namespace MauiApp4.Services
{
    public class GameState
    {
        public string Mode { get; set; } = ""; // "deathmatch" or "catturabandiera"
        public int ModeInt { get; set; } = 0; // di base 0, se maggiore di 0 è una partita in cui vanno messi dei dati come morti o bandiere prese, se minore solo nome squadra e tipo di game
        public int TotalTimeMinutes { get; set; } = 5;
        public string TeamName { get; set; } = "squadra1";
        public int Score { get; set; } = 0; // Deaths or Flags captured
        
        public int RemainingTimeSeconds { get; private set; }
        public bool IsGameRunning { get; private set; } = false;

        private System.Timers.Timer _timer;

        public event Action OnChange;
        public event Action OnTimeUp;

        public GameState()
        {
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += Timer_Elapsed;
        }

        public void StartGame(int timeMinutes)
        {
            TotalTimeMinutes = timeMinutes;
            RemainingTimeSeconds = timeMinutes * 60;
            Score = 0;
            ResumeTimer();
        }

        public void PauseTimer()
        {
            IsGameRunning = false;
            _timer.Stop();
            NotifyStateChanged();
        }

        public void ResumeTimer()
        {
            if (RemainingTimeSeconds > 0)
            {
                IsGameRunning = true;
                _timer.Start();
                NotifyStateChanged();
            }
        }

        public void StopGame()
        {
            IsGameRunning = false;
            _timer.Stop();
            RemainingTimeSeconds = 0; // Azzera il tempo per sicurezza
            NotifyStateChanged();
        }

        public void AddScore()
        {
            Score++;
            NotifyStateChanged();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (RemainingTimeSeconds > 0)
            {
                RemainingTimeSeconds--;
                NotifyStateChanged();

                if (RemainingTimeSeconds <= 0)
                {
                    StopGame();
                    OnTimeUp?.Invoke();
                }
            }
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
