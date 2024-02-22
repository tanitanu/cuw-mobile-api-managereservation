using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable]
    public class FlightSegment : BaseFlightSegment
    {
        private string header = string.Empty;
        private bool isEligibleForPreOrderMeals;
        private bool isPartiallyMealSelected;
        private string errorMessage = string.Empty;

        public string Header
        {
            get { return this.header; }
            set { this.header = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public bool IsEligibleForPreOrderMeals
        {
            get { return this.isEligibleForPreOrderMeals; }
            set { this.isEligibleForPreOrderMeals = value; }
        }

        public bool IsPartiallyMealSelected
        {
            get { return this.isPartiallyMealSelected; }
            set { this.isPartiallyMealSelected = value; }
        }

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { this.errorMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private List<BaseMeal> availableMeals;

        public List<BaseMeal> AvailableMeals
        {
            get { return availableMeals; }
            set { availableMeals = value; }
        }

        private string errorCode;

        public string ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }

        private string flyThankingMessage;

        public string FlyThankingMessage
        {
            get { return flyThankingMessage; }
            set { flyThankingMessage = value; }
        }

        private List<Passenger> passengers;

        public List<Passenger> Passengers
        {
            get { return this.passengers; }
            set { this.passengers = value; }
        }
    }

    [Serializable]
    public class FlightTransit
    {
        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string city;
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        private string transitTime;
        public string TransitTime
        {
            get { return transitTime; }
            set { transitTime = value; }
        }
    }

    [Serializable]
    public class FlightSegmentV2 : BaseFlightSegment
    {
        private string headerDate = string.Empty;
        private string headerSegment = string.Empty;
        private string header = string.Empty;
        private bool isEligibleForPreOrderMeals;
        private bool isPartiallyMealSelected;
        private string errorMessage = string.Empty;

        public string Header
        {
            get { return this.header; }
            set { this.header = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
        public string HeaderDate
        {
            get { return this.headerDate; }
            set { this.headerDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string HeaderSegment
        {
            get { return this.headerSegment; }
            set { this.headerSegment = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public bool IsEligibleForPreOrderMeals
        {
            get { return this.isEligibleForPreOrderMeals; }
            set { this.isEligibleForPreOrderMeals = value; }
        }

        public bool IsPartiallyMealSelected
        {
            get { return this.isPartiallyMealSelected; }
            set { this.isPartiallyMealSelected = value; }
        }

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { this.errorMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private List<BaseMeal> availableMeals;

        public List<BaseMeal> AvailableMeals
        {
            get { return availableMeals; }
            set { availableMeals = value; }
        }

        private string errorCode;

        public string ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }

        private string flyThankingMessage;

        public string FlyThankingMessage
        {
            get { return flyThankingMessage; }
            set { flyThankingMessage = value; }
        }

        private List<PassengerV2> passengers;

        public List<PassengerV2> Passengers
        {
            get
            {
                if (passengers == null)
                {
                    passengers = new List<PassengerV2>();
                }

                return this.passengers;
            }
            set { this.passengers = value; }
        }
    }
}
