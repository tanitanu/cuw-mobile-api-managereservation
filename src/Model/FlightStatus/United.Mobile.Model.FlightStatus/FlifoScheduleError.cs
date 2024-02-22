using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class FlifoScheduleError
    {
        public string Message { get; set; }

        public string Name { get; set; }
    }
}
