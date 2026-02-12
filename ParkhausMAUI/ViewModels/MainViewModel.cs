using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkhausMAUI.Models;
using ParkhausMAUI.Services;
using System.Collections.ObjectModel;

namespace ParkhausMAUI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ParkingService _parkingService;

        [ObservableProperty]
        private ObservableCollection<ParkingLocation> parkings;

        public MainViewModel(ParkingService parkingService)
        {
            _parkingService = parkingService;
            LoadParkings();
        }

        private void LoadParkings() // Parkplätze laden
        {
            var list = _parkingService.GetAvailableParkings();
            Parkings = new ObservableCollection<ParkingLocation>(list);
        }

        [RelayCommand]
        private async Task SelectParking(ParkingLocation location) // Parkhaus auswählen
        {
            if (location == null) return;

            if (location.IsFull)
            {
                await Shell.Current.DisplayAlert("Besetzt", $"Das {location.Name} ist leider aktuell voll besetzt.", "OK");
                return;
            }

            var activeSession = _parkingService.GetActiveSession();
            if (activeSession != null)
            {
                await Shell.Current.DisplayAlert("Hinweis",
                    $"Es läuft bereits ein Parkvorgang im {activeSession.ParkhausName}. Bitte beende diesen zuerst.", "OK");
                await Shell.Current.GoToAsync("///ActiveParkingPage");
                return;
            }

            bool confirm = await Shell.Current.DisplayAlert("Parken starten",
                $"Möchten Sie im {location.Name} parken? (CHF {location.HourlyRate:F2}/h)", "Ja", "Nein");

            if (confirm)
            {
                _parkingService.StartParking(location);
                await Shell.Current.GoToAsync("///ActiveParkingPage");
            }
        }
    }
}