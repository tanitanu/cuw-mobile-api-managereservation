using System;
using System.Collections.Generic;
using United.Mobile.Model.MPSignIn;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpAddPassengersComplete
    {
        private List<MOBEmpBookingItinerary> bookingItineraries;
        private string duplicateBooking;
        private string duplicateBookingMessage;
        private MOBEmployeeProfileExtended employeeProfileExtended;
        private bool mustPrePay;
        private bool isSecureFlightNeeded;
        private bool isTCDMessageNeeded;
        private string totalCost;
        private List<MOBEmpSHOPPrice> empShopPrices = new List<MOBEmpSHOPPrice>();
        
        //In Segments
        //private MOBEmpPSCost empPSCostFromSegments; 
        //private string passClassFromSegments;
       
        //Inside PNRS
        /*PreventTicketing
        TravelPlan
        EmployeeId
        WorkPhone
        HomePhone
        DeliveryType
        DeliveryValue
        BookingPNRSettings
        Errors
        DisplayErrors*/
        public List<MOBEmpSHOPPrice> EmpShopPrices
        {
            get
            {
                return empShopPrices;
            }
            set
            {
                this.empShopPrices = value;
            }
        }

        public List<MOBEmpBookingItinerary> BookingItineraries
        {
            get
            {
                return bookingItineraries;
            }
            set
            {
                this.bookingItineraries = value;
            }
        }
        public string DuplicateBooking
        {
            get
            {
                return duplicateBooking;
            }
            set
            {
                this.duplicateBooking = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string DuplicateBookingMessage
        {
            get
            {
                return duplicateBookingMessage;
            }
            set
            {
                this.duplicateBookingMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public MOBEmployeeProfileExtended EmployeeProfileExtended
        {
            get
            {
                return employeeProfileExtended;
            }
            set
            {
                this.employeeProfileExtended = value;
            }
        }

        public bool MustPrePay
        {
            get
            {
                return mustPrePay;
            }
            set
            {
                this.mustPrePay = value;
            }
        }
        public bool IsSecureFlightNeeded
        {
            get
            {
                return isSecureFlightNeeded;
            }
            set
            {
                this.isSecureFlightNeeded = value;
            }
        }
        public bool IsTCDMessageNeeded
        {
            get
            {
                return isTCDMessageNeeded;
            }
            set
            {
                this.isTCDMessageNeeded = value;
            }
        }
        public string TotalCost
        {
            get
            {
                return totalCost;
            }
            set
            {
                this.totalCost = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
