using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBSearchFilters
    {
        private string aircraftTypes = string.Empty;
        private string airportsDestination;
        private List<MOBSearchFilterItem> airportsDestinationList;
        private string airportsOrigin;
        private List<MOBSearchFilterItem> airportsOriginList;
        private string airportsStop = string.Empty;
        private List<MOBSearchFilterItem> airportsStopList;
        private string airportsStopToAvoid = string.Empty;
        private List<MOBSearchFilterItem> airportsStopToAvoidList;
        private string bookingCodes = string.Empty;
        private int cabinCountMax = -1;
        private int cabinCountMin = -1;
        private bool carrierDefault = true;
        private bool carrierExpress = true;
        private bool carrierPartners = true;
        private string carriersMarketing;
        private List<MOBSearchFilterItem> carriersMarketingList;
        private string carriersOperating;
        private List<MOBSearchFilterItem> carriersOperatingList;
        private bool carrierStar = true;
        private int durationMax = -1;
        private int durationMin = -1;
        private int durationStopMax = -1;
        private int durationStopMin = -1;
        private string equipmentCodes = string.Empty;
        private List<MOBSearchFilterItem> equipmentList;
        private string equipmentTypes;
        private List<MOBSHOPFareFamily> fareFamilies;
        private string fareFamily = "";
        private decimal priceMax = new Decimal(-1.0);
        private decimal priceMin = new Decimal(-1.0);
        private string priceMaxDisplayValue;
        private string priceMinDisplayValue;
        private int stopCountExcl = -1;
        private int stopCountMax = -1;
        private int stopCountMin = -1;
        private string timeArrivalMax = string.Empty;
        private string timeArrivalMin = string.Empty;
        private string timeDepartMax = string.Empty;
        private string timeDepartMin = string.Empty;
        private List<string> warnings;
        private List<MOBSearchFilterItem> warningsFilter;
        private int pageNumber = 1;
        private string sortType1 = string.Empty;
        private List<MOBSearchFilterItem> sortTypes;
        private List<MOBSearchFilterItem> numberofStops;
        private List<MOBSearchFilterItem> amenityTypes;
        private List<MOBSearchFilterItem> carrierTypes;
        private List<MOBSearchFilterItem> aircraftCabinTypes;
        private bool showPriceFilters = false;
        private bool showDepartureFilters = false;
        private bool showArrivalFilters = false;
        private bool showDurationFilters = false;
        private bool showLayOverFilters = false;
        private bool showSortingandFilters = false;
        private bool filterSortPaging = false;
        private string maxArrivalDate;
        private string minArrivalDate;
        private bool showRefundableFaresToggle = false;
        private MOBSearchFilterItem refundableFaresToggle;
        private List<MOBSearchFilterItem> additionalToggles;

        public string AircraftTypes { get { return this.aircraftTypes; } set { this.aircraftTypes = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string AirportsDestination { get; set; }
        public List<MOBSearchFilterItem> AirportsDestinationList { get; set; }
        public string AirportsOrigin { get; set; }
        public List<MOBSearchFilterItem> AirportsOriginList { get; set; }

        public string AirportsStop { get { return this.airportsStop; } set { this.airportsStop = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }

        public List<MOBSearchFilterItem> AirportsStopList { get; set; }
        public string AirportsStopToAvoid { get { return this.airportsStopToAvoid; } set { this.airportsStopToAvoid = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }

        public List<MOBSearchFilterItem> AirportsStopToAvoidList { get; set; }
        public string BookingCodes { get { return this.bookingCodes; } set { this.bookingCodes = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public int CabinCountMax { get; set; }
        public int CabinCountMin { get; set; }
        public bool CarrierDefault { get; set; }
        public bool CarrierExpress { get; set; }
        public bool CarrierPartners { get { return this.carrierPartners; } set { this.carrierPartners = value; } }
        public string CarriersMarketing { get; set; }
        public List<MOBSearchFilterItem> CarriersMarketingList { get; set; }
        public string CarriersOperating { get; set; }
        public List<MOBSearchFilterItem> CarriersOperatingList { get; set; }
        public bool CarrierStar { get; set; }
        public int DurationMax { get; set; }
        public int DurationMin { get; set; }
        public int DurationStopMax { get; set; }
        public int DurationStopMin { get; set; }
        public string EquipmentCodes { get { return this.equipmentCodes; } set { this.equipmentCodes = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<MOBSearchFilterItem> EquipmentList { get; set; }
        public string EquipmentTypes { get; set; }
        public List<MOBSHOPFareFamily> FareFamilies { get; set; }
        public string FareFamily { get { return this.fareFamily; } set { this.fareFamily = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public decimal PriceMax { get; set; }
        public decimal PriceMin { get; set; }
        public string PriceMaxDisplayValue { get; set; }
        public string PriceMinDisplayValue { get; set; }
        public int StopCountExcl { get; set; }
        public int StopCountMax { get; set; }
        public int StopCountMin { get; set; }
        public string TimeArrivalMax { get { return this.timeArrivalMax; } set { this.timeArrivalMax = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string TimeArrivalMin { get { return this.timeArrivalMin; } set { this.timeArrivalMin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string TimeDepartMax { get { return this.timeDepartMax; } set { this.timeDepartMax = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string TimeDepartMin { get { return this.timeDepartMin; } set { this.timeDepartMin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<string> Warnings { get; set; }
        public List<MOBSearchFilterItem> WarningsFilter { get; set; }
        public int PageNumber { get; set; } = 1;
        public string SortType1 { get { return this.sortType1; } set { this.sortType1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }

        public List<MOBSearchFilterItem> SortTypes { get; set; }
        public List<MOBSearchFilterItem> NumberofStops { get; set; }
        public List<MOBSearchFilterItem> AmenityTypes { get; set; }

        public List<MOBSearchFilterItem> AircraftCabinTypes { get; set; }
        public List<MOBSearchFilterItem> CarrierTypes { get; set; }
        public bool ShowPriceFilters { get; set; }
        public bool ShowDepartureFilters { get; set; }
        public bool ShowArrivalFilters { get; set; }
        public bool ShowDurationFilters { get; set; }
        public bool ShowLayOverFilters { get; set; }
        public bool ShowSortingandFilters { get; set; }
        public bool FilterSortPaging { get; set; }
        public string MaxArrivalDate { get; set; }
        public string MinArrivalDate { get; set; }
        public bool ShowRefundableFaresToggle { get; set; }
        public MOBSearchFilterItem RefundableFaresToggle { get; set; }
        public List<MOBSearchFilterItem> AdditionalToggles { get; set; }

    }
}
