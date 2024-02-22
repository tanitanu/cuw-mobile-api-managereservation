using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace United.Mobile.Model.TripPlannerGetService
{
    [Serializable]
    public class MOBSHOPPinDownSegment
    {
        private int flightNumber;
        private string origin = string.Empty;
        private string destination = string.Empty;
        private string departDate = string.Empty;
        private string destinationDate = string.Empty;
        private string carrier = string.Empty;

        [XmlAttribute]
        public int FlightNumber
        {
            get
            {
                return this.flightNumber;
            }
            set
            {
                this.flightNumber = value;
            }
        }

        [XmlAttribute]
        public string Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        [XmlAttribute]
        public string Destination
        {
            get
            {
                return this.destination;
            }
            set
            {
                this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        [XmlAttribute]
        public string DepartDate
        {
            get
            {
                return this.departDate;
            }
            set
            {
                this.departDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        [XmlAttribute]
        public string DestinationDate
        {
            get
            {
                return this.destinationDate;
            }
            set
            {
                this.destinationDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        [XmlAttribute]
        public string Carrier
        {
            get
            {
                return this.carrier;
            }
            set
            {
                this.carrier = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
