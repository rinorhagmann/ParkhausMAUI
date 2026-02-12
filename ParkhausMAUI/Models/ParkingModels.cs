using System;
using System.Collections.Generic;

namespace ParkhausMAUI.Models
{
    public class ParkingLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double HourlyRate { get; set; }
        public int TotalSpaces { get; set; }
        public int OccupiedSpaces { get; set; }
        public int FreeSpaces => Math.Max(0, TotalSpaces - OccupiedSpaces);
        public bool IsFull => FreeSpaces <= 0;
        public bool HasFreeSpaces => !IsFull; 
        public string OccupancyText => IsFull ? "Voll besetzt" : $"{FreeSpaces} Plätze frei";
        public Color OccupancyColor => IsFull ? Colors.Red : Colors.Green;
    }

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
        public ParkingSession CurrentSession { get; set; }
    }
}