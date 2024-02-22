using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable]
    public class PreOrderMealResponseContext : MOBResponse
    {
        private string confirmationNumber = string.Empty;
        private string lastName = string.Empty;
        private string numberOfPassengers = string.Empty;
        private List<FlightSegment> flightSegments;

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

        public List<FlightSegment> FlightSegments
        {
            get
            {
                if (flightSegments == null)
                {
                    flightSegments = new List<FlightSegment>();
                }
                return flightSegments;
            }
            set { this.flightSegments = value; }
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
    }
}
