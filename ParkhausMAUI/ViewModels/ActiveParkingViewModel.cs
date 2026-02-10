using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkhausMAUI.Models;
using ParkhausMAUI.Services;
using System.Collections.ObjectModel;

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

        [ObservableProperty]
        private bool isParkingActive;

        [ObservableProperty]
        private bool isNotParkingActive;

        public ActiveParkingViewModel(ParkingService parkingService)
        {
            _parkingService = parkingService;

            _timer = Application.Current.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => UpdateUI();
        }

        public void OnAppearing()
        {
            CurrentSession = _parkingService.GetActiveSession();

            IsParkingActive = CurrentSession != null;
            IsNotParkingActive = !IsParkingActive;

            if (IsParkingActive)
            {
                _timer.Start();
                UpdateUI();
            }
            else
            {
                _timer.Stop();
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
                double totalHours = duration.TotalHours;
                double costs = totalHours * parking.HourlyRate;
                CurrentCosts = $"CHF {costs:F2}";
            }
        }

        [RelayCommand]
        private async Task StopParking()
        {
            bool answer = await Shell.Current.DisplayAlert("Beenden", "Möchtest du den Parkvorgang wirklich beenden?", "Ja", "Nein");

            if (answer)
            {
                _timer.Stop();
                _parkingService.StopParking();

                OnAppearing();

                await Shell.Current.DisplayAlert("Info", "Parkvorgang wurde im Verlauf gespeichert.", "OK");

            }
        }
    }
}