using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
      [Serializable]
    public class DOTBaggageInfoRequest : MOBRequest
    {
        private List<DOTBaggageFlightSegment> flightSegments;
        private List<DOTBaggageTravelerInfo> traverlerINfo;
        private string recordLocator;
        private string lastName;

        public List<DOTBaggageFlightSegment> FlightSegments
        {
            get { return this.flightSegments; }
            set { this.flightSegments = value; }
        }

        public List<DOTBaggageTravelerInfo> TraverlerINfo
        {
            get { return this.traverlerINfo; }
            set { this.traverlerINfo = value; }
        }

        public string RecordLocator
        {
            get { return this.recordLocator; }
            set { this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string LastName
        {
            get { return this.lastName; }
            set { this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}

