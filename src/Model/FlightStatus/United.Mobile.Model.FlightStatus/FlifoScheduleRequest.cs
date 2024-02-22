using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class FlifoScheduleRequest : MOBRequest
    {
        public FlifoScheduleRequest()
        {
        }

        public FlifoScheduleRequest(string Origin, string Destination, string Date,
                               short Days, string FlightType)
        {
            this.Origin = Origin;
            this.Destination = Destination;
            this.Date = Date;
            this.Days = Days;
            this.FlightType = FlightType;
        }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public string Date { get; set; }

        public short Days { get; set; } 

        public string FlightType { get; set; }

        public string CarrierCode { get; set; }
    }
}
