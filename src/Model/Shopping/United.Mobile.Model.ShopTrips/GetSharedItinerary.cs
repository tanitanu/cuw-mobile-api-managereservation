using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using United.Services.Loyalty.Common.Common;
using United.Services.Loyalty.Preferences.Common;

namespace United.Mobile.Model.ShopTrips
{
    public class GetSharedItineraryDataModel
    {

        public GetSharedItineraryDataModel() { }

        [DataMember(IsRequired = true)]
        [XmlElement(IsNullable = false)]
        public SerializableSharedItinerary SharedItinerary { get; set; }
        [DataMember(IsRequired = true)]
        [XmlElement(IsNullable = false)]
        public string InsertTimestamp { get; set; }
        [DataMember(IsRequired = true)]
        [XmlElement(IsNullable = false)]
        public string UpdateTimestamp { get; set; }
        [DataMember(IsRequired = true)]
        [XmlElement(IsNullable = false)]
        public string InsertID { get; set; }
        [DataMember(IsRequired = true)]
        [XmlElement(IsNullable = false)]
        public string UpdateID { get; set; }
    }
    public class SerializableSharedItinerary
    {
        

        [DataMember(IsRequired = true)]
        [XmlElement(IsNullable = false)]
        public string AccessCode { get; set; }
        [DataMember(IsRequired = false)]
        [XmlElement(IsNullable = true)]
        public bool AwardTravel { get; set; }
        [DataMember(IsRequired = false)]
        [XmlElement(IsNullable = true)]
        public string ChannelType { get; set; }
        [DataMember(IsRequired = false)]
        [XmlElement(IsNullable = true)]
        public string CountryCode { get; set; }
        [DataMember(IsRequired = false)]
        [XmlElement(IsNullable = true)]
        public bool InitialShop { get; set; }
        [DataMember(IsRequired = false)]
        [XmlElement(IsNullable = true)]
        public string LangCode { get; set; }
        [DataMember(IsRequired = false)]
        [XmlElement(IsNullable = true)]
        public string LoyaltyId { get; set; }
        [DataMember(IsRequired = false)]
        [XmlElement(IsNullable = true)]
        public bool NGRP { get; set; }
        [DataMember(IsRequired = false)]
        [XmlElement(IsNullable = true)]
        public List<PaxInfo> PaxInfoList { get; set; }
        [DataMember(IsRequired = false)]
        [XmlElement(IsNullable = true)]
        public SearchType SearchTypeSelection { get; set; }
        [DataMember(IsRequired = false)]
        [XmlElement(IsNullable = true)]
        public bool TrueAvailability { get; set; }
        [DataMember(IsRequired = false)]
        [XmlElement(IsNullable = true)]
        public List<Trip> Trips { get; set; }
    }
    public class PaxInfo
    {
        public PaxInfo() { }

        public string DateOfBirth { get; set; }
        public PaxType PaxType { get; set; }
        public string PaxTypeCode { get; set; }
        public string PaxTypeDescription { get; set; }
    }

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
        public Flight() { }

        public string BookingCode { get; set; }
        public string DepartDateTime { get; set; }
        public string Destination { get; set; }
        public string FlightNumber { get; set; }
        public string MarketingCarrier { get; set; }
        public string Origin { get; set; }
        public string ProductType { get; set; }
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string CurrencyType { get; set; }
        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public decimal? Price { get; set; }
        public List<Flight> Connections { get; set; }
    }
    public class Trip
    {
        public Trip() { }

        public List<Flight> Flights { get; set; }
        public int FlightCount { get; set; }
        public string Destination { get; set; }
        public string Origin { get; set; }
        public string DepartDate { get; set; }
        public string DepartTime { get; set; }
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public int NumberOfStop { get; set; }
    }
    public class SharedItineraryDataModel : SubCommonData
    {
        public SharedItineraryDataModel() { }

        [DataMember(IsRequired = true)]
        [XmlElement(IsNullable = false)]
        public List<GetSharedItineraryDataModel> SharedItineraryList { get; set; }
    }

}
