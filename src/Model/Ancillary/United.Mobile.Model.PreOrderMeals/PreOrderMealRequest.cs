using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable]
    public class BasePreOrderMealRequest : MOBRequest
    {
        private string confirmationNumber;

        public string ConfirmationNumber
        {
            get { return confirmationNumber; }
            set { confirmationNumber = value; }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private string tripNumber;

        public string TripNumber
        {
            get { return tripNumber; }
            set { tripNumber = value; }
        }

        private int segmentNumber;

        public int SegmentNumber
        {
            get { return segmentNumber; }
            set { segmentNumber = value; }
        }

        private string flightNumber;

        public string FlightNumber
        {
            get { return flightNumber; }
            set { flightNumber = value; }
        }

        private string sharesPosition;
        public string SharesPosition
        {
            get { return sharesPosition; }
            set { sharesPosition = value; }
        }

        private string departure;

        public string Departure
        {
            get { return departure; }
            set { departure = value; }
        }

        private string departureDate;

        public string DepartureDate
        {
            get { return departureDate; }
            set { departureDate = value; }
        }

        private string arrival;

        public string Arrival
        {
            get { return arrival; }
            set { arrival = value; }
        }

        private string arrivalDate;

        public string ArrivalDate
        {
            get { return arrivalDate; }
            set { arrivalDate = value; }
        }

        private string sessionId;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        private string applicationId;

        public string ApplicationId
        {
            get { return applicationId; }
            set { applicationId = value; }
        }
        private string givenName;

        public string GivenName
        {
            get { return givenName; }
            set { givenName = value; }
        }

        private string passengerId;
        public string PassengerId
        {
            get { return passengerId; }
            set { passengerId = value; }
        }
    }

    [Serializable]
    public class PreOrderMealCartRequest : BasePreOrderMealRequest
    {
        private string mealCode;

        public string MealCode
        {
            get { return mealCode; }
            set { mealCode = value; }
        }

        private string mealDescription;

        public string MealDescription
        {
            get { return mealDescription; }
            set { mealDescription = value; }
        }

        private string mealServiceCode;

        public string MealServiceCode
        {
            get { return mealServiceCode; }
            set { mealServiceCode = value; }
        }

        private string aircraftModelCode;

        public string AircraftModelCode
        {
            get { return aircraftModelCode; }
            set { aircraftModelCode = value; }
        }

        private string operatingAirlineCode;

        public string OperatingAirlineCode
        {
            get { return operatingAirlineCode; }
            set { operatingAirlineCode = value; }
        }

        private int mealSourceType;

        public int MealSourceType
        {
            get { return mealSourceType; }
            set { mealSourceType = value; }
        }

        private string serviceSequenceNumber;

        public string ServiceSequenceNumber
        {
            get { return serviceSequenceNumber; }
            set { serviceSequenceNumber = value; }
        }

        private string orderId;

        public string OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        private int offerQuantity;

        public int OfferQuantity
        {
            get { return offerQuantity; }
            set { offerQuantity = value; }
        }

        private string offerProvisionType;

        public string OfferProvisionType
        {
            get { return offerProvisionType; }
            set { offerProvisionType = value; }
        }
    }

    [Serializable]
    public class PreOrderMealListRequest : BasePreOrderMealRequest
    {
        private string givenName;

        public string GivenName
        {
            get { return givenName; }
            set { givenName = value; }
        }
        private string appVersion;

        public string AppVersion
        {
            get { return appVersion; }
            set { appVersion = value; }
        }

        private string mealType;

        public string MealType
        {
            get { return mealType; }
            set { mealType = value; }
        }


    }
}
