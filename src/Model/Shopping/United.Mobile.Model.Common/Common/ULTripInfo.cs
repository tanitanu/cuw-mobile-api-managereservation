using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ULTripInfo
    {
        private List<MOBULAirReservation> airReservations;
        private List<MOBULTraveler> travelers;
        private List<MOBULOrderLine> orderLines;
        private int orderAmount;
        private string email;
        private string phoneNumber;

        public List<MOBULAirReservation> AirReservations
        {
            get { return airReservations; }
            set { airReservations = value; }
        }

        public List<MOBULTraveler> Travelers
        {
            get { return travelers; }
            set { travelers = value; }
        }
        public List<MOBULOrderLine> OrderLines
        {
            get { return orderLines; }
            set { orderLines = value; }
        }

        public int OrderAmount
        {
            get { return orderAmount; }
            set { orderAmount = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
    }

    [Serializable()]
    public class MOBULAirReservation
    {
        private string pnr;
        private string reservationType;
        private string tripType;
        private List<MOBULItinerary> itineraries;
        private int price;
        private string origin;
        private string destination;

        public string Pnr
        {
            get { return pnr; }
            set { pnr = value; }
        }

        public string ReservationType
        {
            get { return reservationType; }
            set { reservationType = value; }
        }

        public string TripType
        {
            get { return tripType; }
            set { tripType = value; }
        }

        public List<MOBULItinerary> Itineraries
        {
            get { return itineraries; }
            set { itineraries = value; }
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }
    }

    [Serializable()]
    public class MOBULItinerary
    {
        private string origin;
        private string originDescription;
        private string destination;
        private string destinationDescription;
        private string departureTime;
        private string arrivalTime;
        private string fareClass;
        private string carrierCode;

        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public string OriginDescription
        {
            get { return originDescription; }
            set { originDescription = value; }
        }

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public string DestinationDescription
        {
            get { return destinationDescription; }
            set { destinationDescription = value; }
        }

        public string DepartureTime
        {
            get { return departureTime; }
            set { departureTime = value; }
        }

        public string ArrivalTime
        {
            get { return arrivalTime; }
            set { arrivalTime = value; }
        }

        public string FareClass
        {
            get { return fareClass; }
            set { fareClass = value; }
        }

        public string CarrierCode
        {
            get { return carrierCode; }
            set { carrierCode = value; }
        }
    }

    [Serializable()]
    public class MOBULTraveler
    {
        private int index;
        private string firstName;
        private string lastName;
        private string dateOfBirth;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }
    }

    [Serializable()]
    public class MOBULOrderLine
    {
        private string name;
        private int price;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}
