using System;
using System.Collections.Generic;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class FlifoCitySearchSchedule
    {
        public string Date { get; set; }

        public List<FlifoCitySearchTrip> Trips { get; set; } = new List<FlifoCitySearchTrip>();

        public AirportAdvisoryMessage AirportAdvisoryMessage { get; set; }
    }
}
