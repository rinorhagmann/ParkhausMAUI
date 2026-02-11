using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkhausMAUI.Models;
using ParkhausMAUI.Services;
using System.Collections.ObjectModel;

namespace ParkhausMAUI.ViewModels
{
    public partial class HistoryViewModel : ObservableObject
    {
        private readonly ParkingService _parkingService; 

        [ObservableProperty]
        private ObservableCollection<ParkingSession> historyItems; 

        public HistoryViewModel(ParkingService parkingService)
        {
            _parkingService = parkingService;
            HistoryItems = new ObservableCollection<ParkingSession>();
        }

        public void OnAppearing()
        {
            // Liste beim Öffnen der Seite aktualisieren
            var items = _parkingService.GetHistory();
            HistoryItems = new ObservableCollection<ParkingSession>(items.OrderByDescending(x => x.StartTime));
        }

        [RelayCommand]
        private async Task DeleteEntry(ParkingSession session)
        {
            if (session == null) return;

            // Bestätigungsanforderung
            bool confirm = await Shell.Current.DisplayAlert(
                "Löschen bestätigen",
                $"Möchtest du den Parkvorgang im {session.ParkhausName} wirklich aus dem Verlauf entfernen?",
                "Löschen",
                "Abbrechen");

            if (confirm)
            {
                HistoryItems.Remove(session);

                _parkingService.GetHistory().Remove(session);

                _parkingService.SaveToFile();
            }
        }
    }
}