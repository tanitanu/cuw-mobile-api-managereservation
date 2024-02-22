using System.Collections.Generic;
using United.Service.Presentation.PersonalizationModel;
using United.Services.FlightShopping.Common.FlightReservation;

namespace United.Mobile.Model.UnfinishedBooking
{
    public class CCEContextualElement
    {
        public string Type;
        public Value Value { get; set; }
        public int Rank { get; set; }
    }
    public class Value
    {
     
        public string Message { get; set; }
        public string MessageKey { get; set; }
        public string MessageType { get; set; }   
        public CCEItinerary Itinerary { get; set; }       
        public ContextualContent Content { get; set; }
        public FlightReservationResponse FlightReservationData { get; set; }
    }
    public class CCEItinerary
    {       
        public string ItineraryDisplayPrice { get; set; }     
        public bool IsELF { get; set; }        
        public bool IsIBE { get; set; }       
        public int NumberOfChildren12To17 { get; set; }      
        public int NumberOfInfantWithSeat { get; set; }       
        public int NumberOfChildren5To11 { get; set; }     
        public int NumberOfInfantOnLap { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfSeniors { get; set; }
        public int NumberOfChildren2To4 { get; set; }
        public string ItineraryUnitPrice { get; set; }
        public string InsertTimestamp { get; set; }
        public string UpdateTimestamp { get; set; }
        public string InsertID { get; set; }
        public string UpdateID { get; set; }
        public CCESavedItinerary SavedItinerary { get; set; }
    }
    public class CCESavedItinerary
    {
        public List<Trip> Trips { get; set; }
        public bool TrueAvailability { get; set; }
        public SearchType SearchTypeSelection { get; set; }
        public List<PaxInfo> PaxInfoList { get; set; }
        public string LoyaltyId { get; set; }
        public bool NGRP { get; set; }
        public bool InitialShop { get; set; }
        public string CountryCode { get; set; }
        public string ChannelType { get; set; }
        public bool AwardTravel { get; set; }
        public string AccessCode { get; set; }
        public string LangCode { get; set; }

        public enum PaxType
        {
            ValueNotSet = 0,
            Adult = 1,
            Child01 = 2,
            Child02 = 3,
            Child03 = 4,
            InfantLap = 5,
            InfantSeat = 6,
            Senior = 7,
            Child04 = 8,
            Child05 = 9
        }
        public enum SearchType
        {
            ValueNotSet = 0,
            OneWay = 1,
            RoundTrip = 2,
            MultipleDestination = 3
        }

        public class Flight
        {
            public string BookingCode { get; set; }
            public string DepartDateTime { get; set; }
            public string Destination { get; set; }
            public string FlightNumber { get; set; }
            public string MarketingCarrier { get; set; }
            public string Origin { get; set; }
            public string ProductType { get; set; }
            public List<Product> Products { get; set; }
            public List<Flight> Connections { get; set; }
        }
        public class PaxInfo
        {
            public string DateOfBirth { get; set; }
            public PaxType PaxType { get; set; }
            public string PaxTypeCode { get; set; }
            public string PaxTypeDescription { get; set; }
        }
        public class Trip
        {
            public List<Flight> Flights { get; set; }
            public int FlightCount { get; set; }
            public string Destination { get; set; }
            public string Origin { get; set; }
            public string DepartDate { get; set; }
            public string DepartTime { get; set; }
            public string ArrivalDate { get; set; }
            public string ArrivalTime { get; set; }
        }

        public class Product
        {
            public int TripIndex { get; set; }
            public string ProductType { get; set; }
            public string BookingCode { get; set; }
            public List<Price> Prices { get; set; }
        }
        public class Price
        {

            public bool Selected { get; set; }

            public string PricingType { get; set; }
            public string OfferID { get; set; }
            public string CurrencyAllPax { get; set; }
            public string Currency { get; set; }
            public decimal AmountBase { get; set; }
            public decimal AmountAllPax { get; set; }
            public decimal Amount { get; set; }
            public MerchPriceDetail MerchPriceDetail { get; set; }
            public List<SegmentMapping> SegmentMappings { get; set; }

        }
        public class MerchPriceDetail
        {
            public string ProductCode { get; set; }
            public string EddCode { get; set; }
        }
        public class SegmentMapping
        {
            public string SegmentRefID { get; set; }
            public string CabinDescription { get; set; }
            public string FlightNumber { get; set; }
            public string UpgradeType { get; set; }
            public string UpgradeTo { get; set; }
            public string UpgradeStatus { get; set; }
            public string BBxHash { get; set; }
            public string Destination { get; set; }
            public string Origin { get; set; }
        }
    }
}
