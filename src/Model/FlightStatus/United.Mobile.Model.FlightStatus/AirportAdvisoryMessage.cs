using System;
using System.Collections.Generic;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class AirportAdvisoryMessage
    {
        public string ButtonTitle { get; set; }
        public string HeaderTitle { get; set; }
        public List<TypeOption> AdvisoryMessages { get; set; }
        public AirportAdvisoryMessage()
        {
            AdvisoryMessages = new List<TypeOption>();
        }


    }

    public class AirportAdvisoryMsg
    {
        public AdvisoryKeyValue DepAirportCode { get; set; }
        public AdvisoryKeyValue ArrAirportCode { get; set; }
        public AdvisoryKeyValue Message { get; set; }
        public AdvisoryKeyValue StartDate { get; set; }
        public AdvisoryKeyValue EndDate { get; set; }
        public AdvisoryKeyValue ButtonTitle { get; set; }
        public AdvisoryKeyValue HeaderTitle { get; set; }
    }

    public class AdvisoryKeyValue
    {
        public string S { get; set; }
    }
}
