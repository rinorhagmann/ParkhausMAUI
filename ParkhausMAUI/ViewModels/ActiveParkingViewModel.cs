using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkhausMAUI.Models;
using ParkhausMAUI.Services;

namespace ParkhausMAUI.ViewModels
{
    public partial class ActiveParkingViewModel : ObservableObject
    {
        private readonly ParkingService _parkingService;
        private IDispatcherTimer _timer;

        [ObservableProperty]
        private ParkingSession currentSession;

        [ObservableProperty]
        private string elapsedTime;

        [ObservableProperty]
        private string currentCosts;

        public ActiveParkingViewModel(ParkingService parkingService)
        {
            _parkingService = parkingService;

            // Timer
            _timer = Application.Current.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => UpdateUI();
        }

        // Aufrufen, wenn Tab "Aktuell" öffnet
        public void OnAppearing()
        {
            CurrentSession = _parkingService.GetActiveSession();
            if (CurrentSession != null)
            {
                _timer.Start();
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            if (CurrentSession == null) return;

            var duration = DateTime.Now - CurrentSession.StartTime;
            ElapsedTime = $"{duration.Hours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}";

            var parking = _parkingService.GetAvailableParkings()
                          .FirstOrDefault(p => p.Name == CurrentSession.ParkhausName);

            if (parking != null)
            {
                // Berechnung Preis
                double totalHours = duration.TotalHours;
                double costs = totalHours * parking.HourlyRate;
                CurrentCosts = $"CHF {costs:F2}";
            }
        }

        [RelayCommand]
        private async Task StopParking()
        {
            await Shell.Current.DisplayAlert("Info", "Parken beendet!", "OK");
        }
    }
}