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
    }
}