using System;
using System.Collections.Generic;

namespace ParkhausMAUI.Models
{
    // Model für nötige Parkhausinformationen
    public class ParkingLocation 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double HourlyRate { get; set; }
        public int TotalSpaces { get; set; }
        public int OccupiedSpaces { get; set; }
        public int FreeSpaces => Math.Max(0, TotalSpaces - OccupiedSpaces); // Berechnung der freien Plätze
        public bool IsFull => FreeSpaces <= 0; // überprüfen, ob Parkhaus voll
        public bool HasFreeSpaces => !IsFull; 
        public string OccupancyText => IsFull ? "Voll besetzt" : $"{FreeSpaces} Plätze frei"; // Text für Belegung
        public Color OccupancyColor => IsFull ? Colors.Red : Colors.Green; // Farbe für Belegung
    }

    // Model für aktueller Parkvorgang und Verlauf
    public class ParkingSession
    {
        public string ParkhausName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double TotalCost { get; set; }
        public bool IsActive => EndTime == null; // überprüfen, ob Parkvorgang aktiv ist
    }

    // Model für JSON-Daten
    public class RootData
    {
        public List<ParkingLocation> AvailableParkings { get; set; } = new(); // Liste der verfügbaren Parkhäuser
        public List<ParkingSession> History { get; set; } = new(); // Liste der Parkvorgänge im Verlauf
        public ParkingSession CurrentSession { get; set; } // aktueller Parkvorgang
    }
}