using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable]
    public class PreOrderMealTripDetailResponseContext : MOBResponse
    {
        private string confirmationNumber = string.Empty;
        public string ConfirmationNumber
        {
            get
            {
                return this.confirmationNumber;
            }
            set
            {
                this.confirmationNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string lastName = string.Empty;
        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string numberOfPassengers = string.Empty;
        public string NumberOfPassengers
        {
            get
            {
                return this.numberOfPassengers;
            }
            set
            {
                this.numberOfPassengers = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string footerDescription;

        public string FooterDescription
        {
            get { return footerDescription; }
            set { footerDescription = value; }
        }

        private string footerDescriptionHtmlContent;

        public string FooterDescriptionHtmlContent
        {
            get { return footerDescriptionHtmlContent; }
            set { footerDescriptionHtmlContent = value; }
        }

        private bool isAnySegmentEligibleForPreOrderMeal;

        public bool IsAnySegmentEligibleForPreOrderMeal
        {
            get { return isAnySegmentEligibleForPreOrderMeal; }
            set { isAnySegmentEligibleForPreOrderMeal = value; }
        }

        private string sessionId;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        private List<FlightSegmentV2> flightSegments;
        public List<FlightSegmentV2> FlightSegments
        {
            get
            {
                if (flightSegments == null)
                {
                    flightSegments = new List<FlightSegmentV2>();
                }
                return flightSegments;
            }
            set { this.flightSegments = value; }
        }
    }
}
