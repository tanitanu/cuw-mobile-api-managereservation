using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.UnfinishedBooking
{
    [Serializable()]
    public class MOBSHOPUnfinishedBooking
    {
        #region Variables

        private string cartId = string.Empty;
        private bool isELF;
        private string displayPrice;
        private string countryCode = "US";
        private string searchType = string.Empty;
        private int numberOfAdults;
        private int numberOfSeniors;
        private int numberOfChildren2To4;
        private int numberOfChildren5To11;
        private int numberOfChildren12To17;
        private int numberOfInfantOnLap;
        private int numberOfInfantWithSeat;
        private List<MOBSHOPUnfinishedBookingTrip> trips;
        private string searchExecutionDate;
        private string id;
        private string channelType;
        private bool isIBE;
        private List<MOBTravelerType> travelerTypes;
        private bool isSharedFlightRequest;
        private List<MOBItem> catalogItems;
        #endregion

        #region Properties

        public List<MOBTravelerType> TravelerTypes
        {
            get { return travelerTypes; }
            set { travelerTypes = value; }
        }

        public string CartId
        {
            get { return cartId; }
            set { cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        /// <summary>
        /// True if is BE
        /// </summary>
        public bool IsELF
        {
            get { return isELF; }
            set { isELF = value; }
        }

        public bool IsIBE
        {
            get { return isIBE; }
            set { isIBE = value; }
        }

        /// <summary>
        /// Includes $ sign and is integer
        /// </summary>
        public string DisplayPrice
        {
            get { return displayPrice; }
            set { displayPrice = value; }
        }

        /// <example>
        /// "US"
        /// </example>
        public string CountryCode
        {
            get
            {
                return countryCode;
            }
            set
            {
                countryCode = string.IsNullOrEmpty(value) ? "US" : value.Trim().ToUpper();
            }
        }

        /// <example>
        /// "RT"
        /// </example>
        public string SearchType
        {
            get
            {
                return searchType;
            }
            set
            {
                searchType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int NumberOfAdults
        {
            get
            {
                return numberOfAdults;
            }
            set
            {
                numberOfAdults = value;
            }
        }

        public int NumberOfSeniors
        {
            get
            {
                return numberOfSeniors;
            }
            set
            {
                numberOfSeniors = value;
            }
        }

        public int NumberOfChildren2To4
        {
            get
            {
                return numberOfChildren2To4;
            }
            set
            {
                numberOfChildren2To4 = value;
            }
        }

        public int NumberOfChildren5To11
        {
            get
            {
                return numberOfChildren5To11;
            }
            set
            {
                numberOfChildren5To11 = value;
            }
        }

        public int NumberOfChildren12To17
        {
            get
            {
                return numberOfChildren12To17;
            }
            set
            {
                numberOfChildren12To17 = value;
            }
        }

        public int NumberOfInfantOnLap
        {
            get
            {
                return numberOfInfantOnLap;
            }
            set
            {
                numberOfInfantOnLap = value;
            }
        }

        public int NumberOfInfantWithSeat
        {
            get
            {
                return numberOfInfantWithSeat;
            }
            set
            {
                numberOfInfantWithSeat = value;
            }
        }

        public List<MOBSHOPUnfinishedBookingTrip> Trips
        {
            get { return trips; }
            set { trips = value; }
        }

        /// <example>
        /// "3/22/2018 9:20:01 PM"
        /// </example>
        public string SearchExecutionDate
        {
            get
            {
                return searchExecutionDate;
            }
            set
            {
                searchExecutionDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        /// <example>
        /// A GUID
        /// </example>
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        /// <summary>
        /// Possible: 'MOBILE', 'WEB', null
        /// </summary>
        public string ChannelType
        {
            get { return channelType; }
            set { channelType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }



        public bool IsSharedFlightRequest
        {
            get { return isSharedFlightRequest; }
            set { isSharedFlightRequest = value; }
        }

        public List<MOBItem> CatalogItems
        {
            get { return catalogItems; }
            set { catalogItems = value; }
        }

        #endregion

    }
}
