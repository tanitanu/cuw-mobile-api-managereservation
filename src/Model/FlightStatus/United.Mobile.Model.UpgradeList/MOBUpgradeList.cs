using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.UpgradeList
{
    [Serializable]
    public class MOBUpgradeList
    {
        private MOBAirline airline;
        private MOBSegment segment;
        private int numberOfCabins;
        private MOBUpgradeListCabinClass first;
        private MOBUpgradeListCabinClass business;
        private string departureAirportTimeStamp = string.Empty;
        private bool departed;

        public MOBAirline Airline
        {
            get
            {
                return this.airline;
            }
            set
            {
                this.airline = value;
            }
        }

        public MOBSegment Segment
        {
            get
            {
                return this.segment;
            }
            set
            {
                this.segment = value;
            }
        }

        public int NumberOfCabins
        {
            get
            {
                return this.numberOfCabins;
            }
            set
            {
                this.numberOfCabins = value;
            }
        }

        public MOBUpgradeListCabinClass First
        {
            get
            {
                return this.first;
            }
            set
            {
                this.first = value;
            }
        }

        public MOBUpgradeListCabinClass Business
        {
            get
            {
                return this.business;
            }
            set
            {
                this.business = value;
            }
        }

        public string DepartureAirportTimeStamp
        {
            get
            {
                return this.departureAirportTimeStamp;
            }
            set
            {
                this.departureAirportTimeStamp = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool Departed
        {
            get
            {
                return departed;
            }
            set
            {
                this.departed = value;
            }
        }
    }
}
