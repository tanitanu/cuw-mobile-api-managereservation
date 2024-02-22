using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.UpgradeList
{
    [Serializable()]
    public class MOBUpgradeListRequest : MOBRequest
    {
        private string flightNumber = string.Empty;
        private string flightDate = string.Empty;
        private string departureAirportCode = string.Empty;
        private bool isPullToRefresh;
        private List<string> encryptedPnrsList;
        private List<string> seatsPNRList;

        public MOBUpgradeListRequest()
            : base()
        {
        }

        public string FlightNumber
        {
            get
            {
                return this.flightNumber;
            }
            set
            {
                this.flightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FlightDate
        {
            get
            {
                return this.flightDate;
            }
            set
            {
                this.flightDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DepartureAirportCode
        {
            get
            {
                return this.departureAirportCode;
            }
            set
            {
                this.departureAirportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public bool IsPullToRefresh
        {
            get { return isPullToRefresh; }
            set { isPullToRefresh = value; }
        }

        public List<string> EncryptedPnrsList
        {
            get { return encryptedPnrsList; }
            set { encryptedPnrsList = value; }
        }

        public List<string> SeatsPNRList
        {
            get { return seatsPNRList; }
            set { seatsPNRList = value; }
        }
    }
}
