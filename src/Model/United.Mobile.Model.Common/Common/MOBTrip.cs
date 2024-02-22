using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBTrip
    {
        private int index;
        private string origin = string.Empty;
        private string originName = string.Empty;
        private string destination = string.Empty;
        private string destinationName = string.Empty;
        private string stops = string.Empty;
        private string journeyTime = string.Empty;
        private string journeyMileage = string.Empty;
        private string departureTime = string.Empty;
        private string arrivalTime = string.Empty;
        private string departureTimeGMT = string.Empty;
        private string arrivalTimeGMT = string.Empty;
        private string arrivalOffset = string.Empty;
        private string serviceMap = string.Empty;
        private string departureTimeSort = string.Empty;
        private string cabinType = string.Empty;
        private string destinationImage = string.Empty;
        private string destinationImageDesc = string.Empty;
        private string tripNumber = string.Empty;
        //private string tripKey;
        private List<string> flightNumber;
        private char[] zeros = new char[] { '0' };
        private MOBDestinationImageOnTop destinationImageOnTop;

        [XmlAttribute]
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                this.index = value;
            }
        }

        [XmlAttribute]
        public string CabinType
        {
            get
            {
                return cabinType;
            }
            set
            {
                this.cabinType = value;
            }
        }

        [XmlAttribute]
        public string Origin
        {
            get
            {
                return origin;
            }
            set
            {
                this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        [XmlAttribute]
        public string OriginName
        {
            get
            {
                return originName;
            }
            set
            {
                this.originName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                if (this.originName.IndexOf('-') != -1)
                {
                    int pos = this.originName.IndexOf('-');
                    this.originName = string.Format("{0})", this.originName.Substring(0, pos).Trim());
                }

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

        public string DestinationName
        {
            get
            {
                return destinationName;
            }
            set
            {
                this.destinationName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                if (this.destinationName.IndexOf('-') != -1)
                {
                    int pos = this.destinationName.IndexOf('-');
                    this.destinationName = string.Format("{0})", this.destinationName.Substring(0, pos).Trim());
                }
            }
        }

        [XmlAttribute]
        public string Stops
        {
            get
            {
                return this.stops;
            }
            set
            {
                this.stops = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        [XmlAttribute]
        public string JourneyTime
        {
            get
            {
                return this.journeyTime;
            }
            set
            {
                this.journeyTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToLower();
            }
        }

        [XmlAttribute]
        public string DepartureTime
        {
            get
            {
                return this.departureTime;
            }
            set
            {
                this.departureTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        [XmlIgnore]
        public string DepartureTimeSort
        {
            get
            {
                return this.departureTimeSort;
            }
            set
            {
                this.departureTimeSort = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        [XmlAttribute]
        public string ArrivalTime
        {
            get
            {
                return this.arrivalTime;
            }
            set
            {
                this.arrivalTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        [XmlAttribute]
        public string DepartureTimeGMT
        {
            get
            {
                return this.departureTimeGMT;
            }
            set
            {
                this.departureTimeGMT = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        [XmlAttribute]
        public string ArrivalTimeGMT
        {
            get
            {
                return this.arrivalTimeGMT;
            }
            set
            {
                this.arrivalTimeGMT = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        [XmlAttribute]
        public string ArrivalOffset
        {
            get
            {
                return this.arrivalOffset;
            }
            set
            {
                this.arrivalOffset = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        [XmlAttribute]
        public string ServiceMap
        {
            get
            {
                return this.serviceMap;
            }
            set
            {
                this.serviceMap = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }


        [XmlAttribute]
        public string JourneyMileage
        {
            get
            {
                return this.journeyMileage;
            }
            set
            {
                this.journeyMileage = string.IsNullOrEmpty(value) ? string.Empty : value.TrimStart(zeros).ToUpper();
            }
        }

        //[XmlIgnore]
        //public string TripKey
        //{
        //    get 
        //    { 
        //        return this.tripKey; 
        //    }
        //    set
        //    {
        //        this.tripKey = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        //    }
        //}

        public List<string> FlightNumber
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
        public string DestinationImage
        {
            get
            {
                return this.destinationImage;
            }
            set
            {
                this.destinationImage = value;
            }
        }
        public string DestinationImageDesc
        {
            get
            {
                return this.destinationImageDesc;
            }
            set
            {
                this.destinationImageDesc = value;
            }
        }

        public string TripNumber
        {
            get
            {
                return this.tripNumber;
            }
            set
            {
                this.tripNumber = value;
            }
        }


        public char[] Zeros
        {
            get
            {
                return this.zeros;
            }
        }

        public MOBDestinationImageOnTop DestinationImageOnTop
        {
            get
            {
                return this.destinationImageOnTop;
            }
            set
            {
                this.destinationImageOnTop = value;
            }
        }
    }
}
