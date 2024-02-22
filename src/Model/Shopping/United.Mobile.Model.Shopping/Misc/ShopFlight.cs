using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ShopFlight
    {
        public decimal Airfare { get; set; }
        public string AlternateServiceClass { get; set; } = string.Empty;
        public string ArrivalOffset { get; set; } = string.Empty;
        public string ArrivalGate { get; set; } = string.Empty;
        public string ArrivalTimeZone { get; set; } = string.Empty;
        public List<ShopClassOfService> Availability { get; set; }
        public string BBXCellId { get; set; } = string.Empty;
        public string BBXHash { get; set; } = string.Empty;
        public string BookingClassAvailability { get; set; } = string.Empty;
        public int CabinCount { get; set; }
        public List<ShopCabin> Cabins { get; set; }
        public string ChangeOfGauge { get; set; } = string.Empty;
        public bool ClassList { get; set; }
        public List<ShopFlight> Connections { get; set; }
        public int ConnectTimeMinutes { get; set; }
        public List<ShopDEI> DEIs { get; set; }
        public string DepartDateTime { get; set; } = string.Empty;
        public string DepartureGate { get; set; } = string.Empty;
        public string DepartureTimeZone { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string DestinationCountryCode { get; set; } = string.Empty;
        public string DestinationDateTime { get; set; } = string.Empty; 
        public string DestinationDescription { get; set; } = string.Empty;
        public SHOPEquipmentDisclosure EquipmentDisclosures { get; set; }
        public bool ExtraSection { get; set; }
        public string FareBasisCode { get; set; } = string.Empty;
        public ShopFlightInfo FlightInfo { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public int GroundTimeMinutes { get; set; }
        public string International { get; set; } = string.Empty;
        public string InternationalCity { get; set; } = string.Empty;
        public bool IsCheapestAirfare { get; set; }
        public bool IsConnection { get; set; }
        public string MarketingCarrier { get; set; } = string.Empty;
        public string MarketingCarrierDescription { get; set; } = string.Empty;
        public string Miles { get; set; } = string.Empty;
        public string NoLocalTraffic { get; set; } = string.Empty;
        public ShopOnTimePerformance OnTimePerformance { get; set; }
        public string OperatingCarrier { get; set; } = string.Empty;
        public string OperatingCarrierDescription { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string OriginalFlightNumber { get; set; } = string.Empty;
        public string OriginCountryCode { get; set; } = string.Empty;
        public string OriginDescription { get; set; } = string.Empty;
        public decimal OtherTaxes { get; set; }
        public string ParentFlightNumber { get; set; } = string.Empty;
        public List<ShopPrice> Prices { get; set; }
        public List<MOBShopReward> Rewards { get; set; }
        public bool Selected { get; set; }
        public string StopDestination { get; set; } = string.Empty;
        public List<ShopFlight> StopInfos { get; set; }
        public int Stops { get; set; }
        public string TicketDesignator { get; set; } = string.Empty;
        public int TravelMinutes { get; set; } 
        public int TravelMinutesTotal { get; set; } 
        public string UpgradableCustomers { get; set; } = string.Empty;
        public ShopFlight()
        {
            Availability = new List<ShopClassOfService>();
            Cabins = new List<ShopCabin>();
            Connections = new List<ShopFlight>();
            DEIs = new List<ShopDEI>();
            Prices = new List<ShopPrice>();
            Rewards = new List<MOBShopReward>();
            StopInfos = new List<ShopFlight>();

        }
    }
}
