using System;
using System.Collections.Generic;

namespace c_sharp_playground.Models.Skanetrafiken
{
    public class TrainDelaysResponse
    {
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public List<Journey> Journeys { get; set; }
    }

    public class Journey
    {
        public JourneyPoint Departure { get; set; }
        public JourneyPoint Arrival { get; set; }
        public bool Delayed { get; set; }
        public bool ReplacementBuses { get; set; }
        public bool Cancelled { get; set; }
        public int Switches { get; set; }
        public List<string> LineTypes { get; set; }
        public bool NoRealTimeAvailable { get; set; }
    }

    public class JourneyPoint
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public int Deviation { get; set; }

    }
}
