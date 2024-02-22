using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class FlifoScheduleResponse
    {
        public string CallTime { get; set; }

        public FlifoScheduleError[] Errors;

        public string Message { get; set; }

        public FlifoScheduleTrip[] Schedule;

        public string ServerName { get; set; }

        public string Status { get; set; }
    }
}