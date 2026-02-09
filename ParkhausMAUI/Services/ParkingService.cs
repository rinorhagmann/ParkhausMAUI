using ParkhausMAUI.Models;
using System.Text.Json;

public class ParkingService
{
    private string _filePath = Path.Combine(FileSystem.AppDataDirectory, "parking_data.json");
    private RootData _data;

    public ParkingService()
    {
        LoadData();
    }

    private void LoadData()
    {
        if (!File.Exists(_filePath))
        {
            // Falls JSON-Datei nicht gefunden diese Mock Daten anzeigen
            _data = new RootData
            {
                AvailableParkings = new List<ParkingLocation> {
                    new() { Id = 1, Name = "Parkhaus 1", HourlyRate = 2.50 },
                    new() { Id = 2, Name = "Parkhaus 2", HourlyRate = 4.00 }
                }
            };
            SaveData();
        }
        else
        {
            var json = File.ReadAllText(_filePath);
            _data = JsonSerializer.Deserialize<RootData>(json);
        }
    }

    public void SaveData()
    {
        var json = JsonSerializer.Serialize(_data);
        File.WriteAllText(_filePath, json);
    }

    // Zugriffsmethoden für die ViewModels
    public List<ParkingLocation> GetLocations() => _data.AvailableParkings;
    public List<ParkingSession> GetHistory() => _data.History;
    public ParkingSession GetActiveSession() => _data.CurrentSession;

    // Startet einen neuen Parkvorgang
    public void StartParking(ParkingLocation location)
    {
        if (_data.CurrentSession != null) throw new Exception("Bereits ein Parkvorgang aktiv!");

        _data.CurrentSession = new ParkingSession
        {
            ParkhausName = location.Name,
            StartTime = DateTime.Now
        };
        SaveData();
    }
}