using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class FlifoScheduleFlight
    {
        public string ArrivalGate { get; set; }

        public string ArrivalOffset { get; set; }

        public string ArrivalTimeZone { get; set; }

        public ShopClassOfService[] Availability = new ShopClassOfService[0];

        public FlifoScheduleCabin[] Cabins = new FlifoScheduleCabin[0];

        public FlifoScheduleDEI[] DEIs = new FlifoScheduleDEI[0];

        public string DepartureDate { get; set; }

        public string DepartureGate { get; set; }

        public string DepartureTimeZone { get; set; }

        public string Destination { get; set; }

        public string DestinationCountryCode { get; set; }

        public string Equipment { get; set; }

        public string EquipmentDesc { get; set; }

        public string ExtraSection { get; set; }

        public FlifoScheduleFlifo FliFo;

        public string FlightNumber { get; set; }

        public string FlightTime { get; set; }

        public string International { get; set; }

        public string Leg { get; set; }

        public FlifoScheduleFlight[] Legs = new FlifoScheduleFlight[0];

        public string MarketingCarrier { get; set; }

        public string Miles { get; set; }

        public FlifoScheduleFlightOnTimePerformance OnTimePerformance;

        public string OperatingCarrier { get; set; }

        public string Origin { get; set; }

        public string OriginCountryCode { get; set; }

        public string ScheduledArrivalTime { get; set; }

        public string ScheduledDepartureTime { get; set; }

        public string ServiceMap { get; set; }

        public string Stops { get; set; }

        public string TripNumber { get; set; }

        public string UpgradableCustomers { get; set; }

        public string OperatingCarrierDescription { get; set; }

    }
}