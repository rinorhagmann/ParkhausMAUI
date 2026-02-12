using System;
using System.Collections.Generic;
using System.Text;

namespace ParkhausMAUI.Models
{
    // Parkhaus Daten
    public class ParkingLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double HourlyRate { get; set; }
    }

    // Parkvorgang-Daten
    public class ParkingSession
    {
        public string ParkhausName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double TotalCost { get; set; }
        public bool IsActive => EndTime == null;
    }

    public class RootData
    {
        public List<ParkingLocation> AvailableParkings { get; set; } = new();
        public List<ParkingSession> History { get; set; } = new();
        public ParkingSession CurrentSession { get; set; } // Für den aktiven Parkvorgang
    }
}