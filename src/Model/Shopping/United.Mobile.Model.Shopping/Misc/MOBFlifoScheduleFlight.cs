using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlifoScheduleFlight
    {
        public string ArrivalGate = string.Empty;

        public string ArrivalOffset = string.Empty;

        public string ArrivalTimeZone = string.Empty;

        public string Availability = string.Empty;

        public FlifoScheduleCabin[] Cabins;

        public FlifoScheduleDEI[] DEIs;

        public string DepartureDate = string.Empty;

        public string DepartureGate = string.Empty;

        public string DepartureTimeZone = string.Empty;

        public string Destination = string.Empty;

        public string DestinationCountryCode = string.Empty;

        public string Equipment = string.Empty;

        public string EquipmentDesc = string.Empty;

        public string ExtraSection = string.Empty;

        public FlifoScheduleFlifo FliFo;

        public string FlightNumber = string.Empty;

        public string FlightTime = string.Empty;

        public string International = string.Empty;

        public string Leg = string.Empty;

        public FlifoScheduleFlight[] Legs;

        public string MarketingCarrier = string.Empty;

        public string Miles = string.Empty;

        public FlifoScheduleFlightOnTimePerformance OnTimePerformance;

        public string OperatingCarrier = string.Empty;

        public string Origin = string.Empty;

        public string OriginCountryCode = string.Empty;

        public string ScheduledArrivalTime = string.Empty;

        public string ScheduledDepartureTime = string.Empty;

        public string ServiceMap = string.Empty;

        public string Stops = string.Empty;

        public string TripNumber = string.Empty;

        public string UpgradableCustomers = string.Empty;

        public string OperatingCarrierDescription = string.Empty;
        ///// <remarks/>
        //public string ArrivalGate
        //{
        //    get
        //    {
        //        return this.arrivalGate;
        //    }
        //    set
        //    {
        //        this.arrivalGate = value;
        //    }
        //}

        ///// <remarks/>
        //public string ArrivalOffset
        //{
        //    get
        //    {
        //        return this.arrivalOffset;
        //    }
        //    set
        //    {
        //        this.arrivalOffset = value;
        //    }
        //}

        ///// <remarks/>
        //public string ArrivalTimeZone
        //{
        //    get
        //    {
        //        return this.arrivalTimeZone;
        //    }
        //    set
        //    {
        //        this.arrivalTimeZone = value;
        //    }
        //}

        ///// <remarks/>
        //public string Availability
        //{
        //    get
        //    {
        //        return this.availability;
        //    }
        //    set
        //    {
        //        this.availability = value;
        //    }
        //}

        ///// <remarks/>
        //public FlifoScheduleCabin[] Cabins
        //{
        //    get
        //    {
        //        return this.cabins;
        //    }
        //    set
        //    {
        //        this.cabins = value;
        //    }
        //}

        ///// <remarks/>
        //public FlifoScheduleDEI[] DEIs
        //{
        //    get
        //    {
        //        return this.dEIs;
        //    }
        //    set
        //    {
        //        this.dEIs = value;
        //    }
        //}

        ///// <remarks/>
        //public string DepartureDate
        //{
        //    get
        //    {
        //        return this.departureDate;
        //    }
        //    set
        //    {
        //        this.departureDate = value;
        //    }
        //}

        ///// <remarks/>
        //public string DepartureGate
        //{
        //    get
        //    {
        //        return this.departureGate;
        //    }
        //    set
        //    {
        //        this.departureGate = value;
        //    }
        //}

        ///// <remarks/>
        //public string DepartureTimeZone
        //{
        //    get
        //    {
        //        return this.departureTimeZone;
        //    }
        //    set
        //    {
        //        this.departureTimeZone = value;
        //    }
        //}

        ///// <remarks/>
        //public string Destination
        //{
        //    get
        //    {
        //        return this.destination;
        //    }
        //    set
        //    {
        //        this.destination = value;
        //    }
        //}

        ///// <remarks/>
        //public string DestinationCountryCode
        //{
        //    get
        //    {
        //        return this.destinationCountryCode;
        //    }
        //    set
        //    {
        //        this.destinationCountryCode = value;
        //    }
        //}

        ///// <remarks/>
        //public string Equipment
        //{
        //    get
        //    {
        //        return this.equipment;
        //    }
        //    set
        //    {
        //        this.equipment = value;
        //    }
        //}

        ///// <remarks/>
        //public string EquipmentDesc
        //{
        //    get
        //    {
        //        return this.equipmentDesc;
        //    }
        //    set
        //    {
        //        this.equipmentDesc = value;
        //    }
        //}

        ///// <remarks/>
        //public string ExtraSection
        //{
        //    get
        //    {
        //        return this.extraSection;
        //    }
        //    set
        //    {
        //        this.extraSection = value;
        //    }
        //}

        ///// <remarks/>
        //public FliFoScheduleFlifo FliFo
        //{
        //    get
        //    {
        //        return this.fliFo;
        //    }
        //    set
        //    {
        //        this.fliFo = value;
        //    }
        //}

        ///// <remarks/>
        //public string FlightNumber
        //{
        //    get
        //    {
        //        return this.flightNumber;
        //    }
        //    set
        //    {
        //        this.flightNumber = value;
        //    }
        //}

        ///// <remarks/>
        //public string FlightTime
        //{
        //    get
        //    {
        //        return this.flightTime;
        //    }
        //    set
        //    {
        //        this.flightTime = value;
        //    }
        //}

        ///// <remarks/>
        //public string International
        //{
        //    get
        //    {
        //        return this.international;
        //    }
        //    set
        //    {
        //        this.international = value;
        //    }
        //}

        ///// <remarks/>
        //public string Leg
        //{
        //    get
        //    {
        //        return this.leg;
        //    }
        //    set
        //    {
        //        this.leg = value;
        //    }
        //}

        ///// <remarks/>
        ////public FlifoScheduleLegs Legs
        ////{
        ////    get
        ////    {
        ////        return this.legs;
        ////    }
        ////    set
        ////    {
        ////        this.legs = value;
        ////    }
        ////}

        ///// <remarks/>
        //public string MarketingCarrier
        //{
        //    get
        //    {
        //        return this.marketingCarrier;
        //    }
        //    set
        //    {
        //        this.marketingCarrier = value;
        //    }
        //}

        ///// <remarks/>
        //public string Miles
        //{
        //    get
        //    {
        //        return this.miles;
        //    }
        //    set
        //    {
        //        this.miles = value;
        //    }
        //}

        ///// <remarks/>
        //public FlifoScheduleFlightOnTimePerformance OnTimePerformance
        //{
        //    get
        //    {
        //        return this.onTimePerformance;
        //    }
        //    set
        //    {
        //        this.onTimePerformance = value;
        //    }
        //}

        ///// <remarks/>
        //public string OperatingCarrier
        //{
        //    get
        //    {
        //        return this.operatingCarrier;
        //    }
        //    set
        //    {
        //        this.operatingCarrier = value;
        //    }
        //}

        ///// <remarks/>
        //public string Origin
        //{
        //    get
        //    {
        //        return this.origin;
        //    }
        //    set
        //    {
        //        this.origin = value;
        //    }
        //}

        ///// <remarks/>
        //public string OriginCountryCode
        //{
        //    get
        //    {
        //        return this.originCountryCode;
        //    }
        //    set
        //    {
        //        this.originCountryCode = value;
        //    }
        //}

        ///// <remarks/>
        //public string ScheduledArrivalTime
        //{
        //    get
        //    {
        //        return this.scheduledArrivalTime;
        //    }
        //    set
        //    {
        //        this.scheduledArrivalTime = value;
        //    }
        //}

        ///// <remarks/>
        //public string ScheduledDepartureTime
        //{
        //    get
        //    {
        //        return this.scheduledDepartureTime;
        //    }
        //    set
        //    {
        //        this.scheduledDepartureTime = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        //public string ServiceMap
        //{
        //    get
        //    {
        //        return this.serviceMap;
        //    }
        //    set
        //    {
        //        this.serviceMap = value;
        //    }
        //}

        ///// <remarks/>
        //public string Stops
        //{
        //    get
        //    {
        //        return this.stops;
        //    }
        //    set
        //    {
        //        this.stops = value;
        //    }
        //}

        ///// <remarks/>
        //public string TripNumber
        //{
        //    get
        //    {
        //        return this.tripNumber;
        //    }
        //    set
        //    {
        //        this.tripNumber = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        //public string UpgradableCustomers
        //{
        //    get
        //    {
        //        return this.upgradableCustomers;
        //    }
        //    set
        //    {
        //        this.upgradableCustomers = value;
        //    }
        //}
    }
}
