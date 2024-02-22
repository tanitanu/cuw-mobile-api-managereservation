using System;
using System.Collections.Generic;
using United.Mobile.Model.BagCalculator;
using United.Mobile.Model.Common;

namespace United.ile.Model.BagCalculator
{
    [Serializable]
    public class DOTBaggageInfoRequest : MOBRequest
    {
        //public List<DOTBaggageFlightSegment> FlightSegments { get; set; }

        private List<DOTBaggageFlightSegment> flightSegments;
        public List<DOTBaggageFlightSegment> FlightSegments
        {
            get { return this.flightSegments; }
            set { this.flightSegments = value; }
        }

        public List<DOTBaggageTravelerInfo> TraverlerINfo { get; set; }
        public string RecordLocator { get; set; } 
        public string LastName { get; set; }
        public DOTBaggageInfoRequest()
        {
            //FlightSegments = new List<DOTBaggageFlightSegment>();
            TraverlerINfo = new List<DOTBaggageTravelerInfo>();
        }
    }
}
