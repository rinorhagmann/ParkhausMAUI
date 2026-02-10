using System.Text.Json;
using ParkhausMAUI.Models;

namespace ParkhausMAUI.Services
{
    public class ParkingService
    {
        private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "parking_basel_v1.json");
        private RootData _data;

        public ParkingService()
        {
            InitializeData();
        }

        private void InitializeData()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    // JSON-Daten mit 25 Basler Parkhäusern
                    _data = new RootData
                    {
                        AvailableParkings = new List<ParkingLocation>
                        {
                            new() { Id = 1, Name = "Parkhaus Centralbahnparking", HourlyRate = 4.00 },
                            new() { Id = 2, Name = "Parkhaus Elisabethen", HourlyRate = 3.00 },
                            new() { Id = 3, Name = "Parkhaus Steinen", HourlyRate = 3.00 },
                            new() { Id = 4, Name = "Parkhaus City", HourlyRate = 3.00 },
                            new() { Id = 5, Name = "Parkhaus Storchen", HourlyRate = 4.50 },
                            new() { Id = 6, Name = "Parkhaus Badischer Bahnhof", HourlyRate = 2.50 },
                            new() { Id = 7, Name = "Parkhaus Messe Basel", HourlyRate = 3.50 },
                            new() { Id = 8, Name = "Parkhaus Rebgasse", HourlyRate = 2.50 },
                            new() { Id = 9, Name = "Parkhaus Claramatte", HourlyRate = 2.50 },
                            new() { Id = 10, Name = "Parkhaus Europe", HourlyRate = 3.50 },
                            new() { Id = 11, Name = "Parkhaus Post Basel", HourlyRate = 3.50 },
                            new() { Id = 12, Name = "Parkhaus Anfos", HourlyRate = 3.50 },
                            new() { Id = 13, Name = "Parkhaus Aeschen", HourlyRate = 3.50 },
                            new() { Id = 14, Name = "Parkhaus Bahnhof Süd", HourlyRate = 3.50 },
                            new() { Id = 15, Name = "Parkhaus St. Jakob", HourlyRate = 2.00 },
                            new() { Id = 16, Name = "Parkhaus Kunstmuseum", HourlyRate = 4.00 },
                            new() { Id = 17, Name = "Parkhaus Clarahuus", HourlyRate = 3.00 },
                            new() { Id = 18, Name = "Parkhaus ELYS", HourlyRate = 2.50 },
                            new() { Id = 19, Name = "Parkhaus Theater", HourlyRate = 3.50 },
                            new() { Id = 20, Name = "Parkhaus Horburg", HourlyRate = 2.00 },
                            new() { Id = 21, Name = "Parkhaus U70", HourlyRate = 2.50 },
                            new() { Id = 22, Name = "Parkhaus Universitätsspital (UKBB)", HourlyRate = 3.00 },
                            new() { Id = 23, Name = "Parkhaus Grosspeter", HourlyRate = 3.00 },
                            new() { Id = 24, Name = "Parkhaus St. Jakobshalle", HourlyRate = 1.50 },
                            new() { Id = 25, Name = "Parkhaus City (Schanzenstrasse)", HourlyRate = 3.00 }
                        },
                        History = new List<ParkingSession>()
                    };
                    SaveToFile();
                }
                else
                {
                    var json = File.ReadAllText(_filePath);
                    _data = JsonSerializer.Deserialize<RootData>(json) ?? new RootData();
                }
            }
            catch (Exception ex)
            {
                _data = new RootData();
                System.Diagnostics.Debug.WriteLine($"Fehler beim Laden: {ex.Message}");
            }
        }

        public void SaveToFile()
        {
            var json = JsonSerializer.Serialize(_data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public List<ParkingLocation> GetAvailableParkings() => _data.AvailableParkings;

        public ParkingSession GetActiveSession()
        {
            return _data.CurrentSession;
        }

        public void StartParking(ParkingLocation location)
        {
            // Session starten
            _data.CurrentSession = new ParkingSession
            {
                ParkhausName = location.Name,
                StartTime = DateTime.Now,
                EndTime = null,
                TotalCost = 0
            };

            SaveToFile();
        }

        public void StopParking()
        {
            if (_data.CurrentSession == null) return;

            // Session beenden
            _data.CurrentSession.EndTime = DateTime.Now;

            // In den Parkverlauf verschieben
            _data.History.Add(_data.CurrentSession);

            // Aktive Session leeren
            _data.CurrentSession = null;

            SaveToFile();
        }
    }
}