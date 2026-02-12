using System.Text.Json;
using ParkhausMAUI.Models;

namespace ParkhausMAUI.Services
{
    public class ParkingService
    {
        private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "parking_basel.json");
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
                    _data = new RootData
                    {
                        AvailableParkings = new List<ParkingLocation>
                        {
                            new() { Id = 1, Name = "Parkhaus Centralbahn", HourlyRate = 4.00, TotalSpaces = 500, OccupiedSpaces = 485 },
                            new() { Id = 2, Name = "Parkhaus Elisabethen", HourlyRate = 3.00, TotalSpaces = 800, OccupiedSpaces = 800 }, // VOLL
                            new() { Id = 3, Name = "Parkhaus Steinen", HourlyRate = 3.00, TotalSpaces = 650, OccupiedSpaces = 120 },
                            new() { Id = 4, Name = "Parkhaus City", HourlyRate = 3.00, TotalSpaces = 400, OccupiedSpaces = 399 },
                            new() { Id = 5, Name = "Parkhaus Storchen", HourlyRate = 4.50, TotalSpaces = 250, OccupiedSpaces = 250 }, // VOLL
                            new() { Id = 6, Name = "Parkhaus Bad. Bahnhof", HourlyRate = 2.50, TotalSpaces = 300, OccupiedSpaces = 45 },
                            new() { Id = 7, Name = "Parkhaus Messe Basel", HourlyRate = 3.50, TotalSpaces = 1200, OccupiedSpaces = 1150 },
                            new() { Id = 8, Name = "Parkhaus Rebgasse", HourlyRate = 2.50, TotalSpaces = 200, OccupiedSpaces = 200 }, // VOLL
                            new() { Id = 9, Name = "Parkhaus Claramatte", HourlyRate = 2.50, TotalSpaces = 150, OccupiedSpaces = 10 },
                            new() { Id = 10, Name = "Parkhaus Europe", HourlyRate = 3.50, TotalSpaces = 350, OccupiedSpaces = 350 }, // VOLL
                            new() { Id = 11, Name = "Parkhaus Post Basel", HourlyRate = 3.50, TotalSpaces = 220, OccupiedSpaces = 180 },
                            new() { Id = 12, Name = "Parkhaus Anfos", HourlyRate = 3.50, TotalSpaces = 450, OccupiedSpaces = 445 },
                            new() { Id = 13, Name = "Parkhaus Aeschen", HourlyRate = 3.50, TotalSpaces = 380, OccupiedSpaces = 380 }, // VOLL
                            new() { Id = 14, Name = "Parkhaus Bahnhof Süd", HourlyRate = 3.50, TotalSpaces = 550, OccupiedSpaces = 540 },
                            new() { Id = 15, Name = "Parkhaus St. Jakob", HourlyRate = 2.00, TotalSpaces = 1000, OccupiedSpaces = 200 },
                            new() { Id = 16, Name = "Parkhaus Clarahuus", HourlyRate = 3.00, TotalSpaces = 180, OccupiedSpaces = 180 }, // VOLL
                            new() { Id = 17, Name = "Parkhaus Universitätsspital", HourlyRate = 3.00, TotalSpaces = 600, OccupiedSpaces = 580 }
                        }
                    };
                    SaveToFile();
                }
                else
                {
                    var json = File.ReadAllText(_filePath);
                    _data = JsonSerializer.Deserialize<RootData>(json) ?? new RootData();
                }
            }
            catch (Exception)
            {
                _data = new RootData();
            }
        }

        public void SaveToFile()
        {
            var json = JsonSerializer.Serialize(_data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public List<ParkingLocation> GetAvailableParkings() => _data.AvailableParkings;
        public ParkingSession GetActiveSession() => _data.CurrentSession;

        public void StartParking(ParkingLocation location)
        {
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
            _data.CurrentSession.EndTime = DateTime.Now;
            var duration = _data.CurrentSession.EndTime.Value - _data.CurrentSession.StartTime;
            var parking = _data.AvailableParkings.FirstOrDefault(p => p.Name == _data.CurrentSession.ParkhausName);

            if (parking != null)
                _data.CurrentSession.TotalCost = duration.TotalHours * parking.HourlyRate;

            _data.History.Add(_data.CurrentSession);
            _data.CurrentSession = null;
            SaveToFile();
        }

        public List<ParkingSession> GetHistory() => _data.History ?? new List<ParkingSession>();
    }
}