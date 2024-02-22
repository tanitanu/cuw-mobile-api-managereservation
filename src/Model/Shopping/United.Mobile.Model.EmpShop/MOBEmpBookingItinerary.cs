using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpBookingItinerary
    {

        private List<MOBEmpBookingTrip> bookingTrips;
        private MOBTypeOption cabin; 
        private MOBEmpTCDInfo empTCDInfo;
        private string displacementCost;
        private bool isBuddy;
        private string itineraryName;
        private string lapChildName;
        private string passengerId;
        private MOBEmpPassengerPrice passengerPrice;
        private MOBEmpPassType passType;
        private MOBEmpRelationship relationship;
        private MOBEmpSSRInfo selectedSSROption;
        

        public List<MOBEmpBookingTrip> BookingTrips
        {
            get
            {
                return bookingTrips;
            }
            set
            {
                this.bookingTrips = value;
            }
        }

        public MOBTypeOption Cabin
        {
            get
            {
                return cabin;
            }
            set
            {
                this.cabin = value;
            }
        }
        public MOBEmpSSRInfo SelectedSSROption
        {
            get
            {
                return selectedSSROption;
            }
            set
            {
                this.selectedSSROption = value;
            }
        }
        public MOBEmpTCDInfo EmpTCDInfo
        {
            get
            {
                return empTCDInfo;
            }
            set
            {
                this.empTCDInfo = value;
            }
        }
        public string DisplacementCost
        {
            get
            {
                return displacementCost;
            }
            set
            {
                this.displacementCost = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool IsBuddy
        {
            get
            {
                return isBuddy;
            }
            set
            {
                this.isBuddy = value;
            }
        }
        public string ItineraryName
        {
            get
            {
                return itineraryName;
            }
            set
            {
                this.itineraryName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }

        }
        public string LapChildName
        {
            get
            {
                return lapChildName;
            }
            set
            {
                this.lapChildName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string PassengerId
        {
            get
            {
                return passengerId;
            }
            set
            {
                this.passengerId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public MOBEmpPassengerPrice PassengerPrice
        {
            get
            {
                return passengerPrice;
            }
            set
            {
                this.passengerPrice = value;
            }
        }
        public MOBEmpPassType PassType
        {
            get
            {
                return passType;
            }
            set
            {
                passType = value;
            }
        }
        public MOBEmpRelationship Relationship
        {
            get
            {
                return relationship;
            }
            set
            {
                relationship = value;
            }
        }
            
    }
}
