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

        [ObservableProperty]
        private bool isParkingActive;

        [ObservableProperty]
        private bool isNotParkingActive;

        [ObservableProperty]
        private bool isPaid;

        [ObservableProperty]
        private bool isNotPaid;

        public ActiveParkingViewModel(ParkingService parkingService)
        {
            _parkingService = parkingService;

            _timer = Application.Current.Dispatcher.CreateTimer(); // Timer
            _timer.Interval = TimeSpan.FromSeconds(1); // Aktualisierung alle 1 Sekunde
            _timer.Tick += (s, e) => UpdateUI(); 
        }

        public void OnAppearing() // Wird aufgerufen, wenn die Seite erscheint
        {
            CurrentSession = _parkingService.GetActiveSession(); 

            IsParkingActive = CurrentSession != null; // Überprüfen, ob ein Parkvorgang aktiv ist
            IsNotParkingActive = !IsParkingActive; 

            if (!IsPaid) // Überprüfen, ob bereits bezahlt wurde
            {
                IsPaid = false;
                IsNotPaid = true;
            }

            if (IsParkingActive && !IsPaid) // Wenn Parkvorgang aktiv und nicht bezahlt, Timer starten
            {
                _timer.Start();
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            if (CurrentSession == null) return;

            var duration = DateTime.Now - CurrentSession.StartTime; // Berechnung der Parkdauer
            ElapsedTime = $"{duration.Hours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}"; // Formatierung der Parkdauer

            var parking = _parkingService.GetAvailableParkings()
                          .FirstOrDefault(p => p.Name == CurrentSession.ParkhausName); // Abrufen Parkhausdaten

            if (parking != null)
            {
                double totalHours = duration.TotalHours;
                double costs = totalHours * parking.HourlyRate; // Berechnung der Kosten
                CurrentCosts = $"CHF {costs:F2}"; // Formatierung der Kosten
            }
        }

        [RelayCommand]
        private async Task PayParking() // Bezahlen des Parkvorgangs
        {
            bool answer = await Shell.Current.DisplayAlert("Bezahlen", $"Möchten Sie den Betrag von {CurrentCosts} jetzt bezahlen?", "Ja", "Nein");

            if (answer)
            {
                _timer.Stop(); 
                IsPaid = true;
                IsNotPaid = false;
                await Shell.Current.DisplayAlert("Erfolg", "Zahlung erfolgreich! Sie können nun ausfahren.", "OK");
            }
        }

        [RelayCommand]
        private async Task StopParking() // Beenden des Parkvorgangs
        {
            _parkingService.StopParking();

            IsPaid = false;
            IsNotPaid = true;

            OnAppearing();

            await Shell.Current.DisplayAlert("Gute Fahrt", "Der Parkvorgang wurde abgeschlossen.", "OK");

            await Shell.Current.GoToAsync("///HistoryPage");
        }
    }
}