using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class FlifoCitySearchResponse : MOBResponse
    {
        public FlifoCitySearchSchedule Schedule { get; set; }
    }
}
