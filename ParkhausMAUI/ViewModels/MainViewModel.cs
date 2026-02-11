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

        private void LoadParkings()
        {
            var list = _parkingService.GetAvailableParkings();
            Parkings = new ObservableCollection<ParkingLocation>(list);
        }


        [RelayCommand]
        private async Task SelectParking(ParkingLocation location)
        {
            if (location == null) return;

            // Prüfung auf aktiven Parkvorgang
            var activeSession = _parkingService.GetActiveSession();
            if (activeSession != null)
            {
                await Shell.Current.DisplayAlert("Hinweis",
                    $"Es läuft bereits ein Parkvorgang im {activeSession.ParkhausName}. Bitte beende diesen zuerst.", "OK");

                await Shell.Current.GoToAsync("///ActiveParkingPage");
                return;
            }

            bool confirm = await Shell.Current.DisplayAlert("Parken starten",
                $"Möchtest du im {location.Name} parken? Der Stundensatz beträgt CHF {location.HourlyRate}.", "Ja", "Nein");

            if (confirm)
            {
                // Neuen Parkvorgang starten
                _parkingService.StartParking(location);

                // Feedback
                await Shell.Current.DisplayAlert("Parken gestartet",
                    $"Dein Parkvorgang im {location.Name} wurde erfolgreich gestartet.", "OK");

                // Automatisch zur ActiveParkingPage navigieren
                await Shell.Current.GoToAsync("///ActiveParkingPage");
            }
        }
    }
}