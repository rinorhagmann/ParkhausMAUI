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

            Parkings = new ObservableCollection<ParkingLocation>(_parkingService.GetLocations());
        }

        [RelayCommand]
        private async Task SelectParking(ParkingLocation location)
        {
            if (location == null) return;

            
            await Shell.Current.DisplayAlert("Auswahl", $"Du hast {location.Name} gewählt.", "OK");
        }
    }
}