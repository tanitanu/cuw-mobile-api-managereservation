using System;
using System.Collections.Generic;

namespace United.Mobile.Model.BagTracking
{
    [Serializable]
    public class PassengerItinerary
    {
        public string ConfirmationNumber { get; set; } = string.Empty;
        public List<Segment> Segments { get; set; }
        public PassengerItinerary()
        {
            Segments = new List<Segment>();
        }
    }
}
