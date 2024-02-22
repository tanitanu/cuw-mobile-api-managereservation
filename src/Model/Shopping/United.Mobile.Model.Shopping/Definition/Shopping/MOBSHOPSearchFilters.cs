using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Persist.Definition.Shopping
{
    [Serializable()]
    public class MOBSHOPSearchFilters
    {
        private string aircraftTypes = string.Empty;
        private string airportsDestination = string.Empty;
        private List<MOBSHOPSearchFilterItem> airportsDestinationList;
        private string airportsOrigin = string.Empty;
        private List<MOBSHOPSearchFilterItem> airportsOriginList;
        private string airportsStop = string.Empty;
        private List<MOBSHOPSearchFilterItem> airportsStopList;
        private string airportsStopToAvoid = string.Empty;
        private List<MOBSHOPSearchFilterItem> airportsStopToAvoidList;
        private string bookingCodes = string.Empty;
        private int cabinCountMax = -1;
        private int cabinCountMin = -1;
        private bool carrierDefault = true;
        private bool carrierExpress = true;
        private bool carrierPartners = true;
        private string carriersMarketing = string.Empty;
        private List<MOBSHOPSearchFilterItem> carriersMarketingList;
        private string carriersOperating = string.Empty;
        private List<MOBSHOPSearchFilterItem> carriersOperatingList;
        private bool carrierStar = true;
        private int durationMax = -1;
        private int durationMin = -1;
        private int durationStopMax = -1;
        private int durationStopMin = -1;
        private string equipmentCodes = string.Empty;
        private List<MOBSHOPSearchFilterItem> equipmentList;
        private string equipmentTypes = string.Empty;
        private List<MOBSHOPFareFamily> fareFamilies;
        private string fareFamily = "";
        private decimal priceMax = new Decimal(-1.0);
        private decimal priceMin = new Decimal(-1.0);
        private string priceMaxDisplayValue = string.Empty;
        private string priceMinDisplayValue = string.Empty;
        private int stopCountExcl = -1;
        private int stopCountMax = -1;
        private int stopCountMin = -1;
        private string timeArrivalMax = string.Empty;
        private string timeArrivalMin = string.Empty;
        private string timeDepartMax = string.Empty;
        private string timeDepartMin = string.Empty;
        private List<string> warnings;
        private List<MOBSHOPSearchFilterItem> warningsFilter;
        private int pageNumber = 1;
        private string sortType1 = string.Empty;
        private List<MOBSHOPSearchFilterItem> sortTypes;
        private List<MOBSHOPSearchFilterItem> numberofStops;
        private List<MOBSHOPSearchFilterItem> amenityTypes;
        private List<MOBSHOPSearchFilterItem> carrierTypes;
        private List<MOBSHOPSearchFilterItem> aircraftCabinTypes;
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
        private MOBSHOPSearchFilterItem refundableFaresToggle;
        private List<MOBSHOPSearchFilterItem> additionalToggles;

        public string AircraftTypes { get { return this.aircraftTypes; } set { this.aircraftTypes = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string AirportsDestination { get { return this.airportsDestination; } set { this.airportsDestination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<MOBSHOPSearchFilterItem> AirportsDestinationList { get { return this.airportsDestinationList; } set { this.airportsDestinationList = value; } }
        public string AirportsOrigin { get { return this.airportsOrigin; } set { this.airportsOrigin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<MOBSHOPSearchFilterItem> AirportsOriginList { get { return this.airportsOriginList; } set { this.airportsOriginList = value; } }
        public string AirportsStop { get { return this.airportsStop; } set { this.airportsStop = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<MOBSHOPSearchFilterItem> AirportsStopList { get { return this.airportsStopList; } set { this.airportsStopList = value; } }
        public string AirportsStopToAvoid { get { return this.airportsStopToAvoid; } set { this.airportsStopToAvoid = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<MOBSHOPSearchFilterItem> AirportsStopToAvoidList { get { return this.airportsStopToAvoidList; } set { this.airportsStopToAvoidList = value; } }
        public string BookingCodes { get { return this.bookingCodes; } set { this.bookingCodes = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public int CabinCountMax { get { return this.cabinCountMax; } set { this.cabinCountMax = value; } }
        public int CabinCountMin { get { return this.cabinCountMin; } set { this.cabinCountMin = value; } }
        public bool CarrierDefault { get { return this.carrierDefault; } set { this.carrierDefault = value; } }
        public bool CarrierExpress { get { return this.carrierExpress; } set { this.carrierExpress = value; } }
        public bool CarrierPartners { get { return this.carrierPartners; } set { this.carrierPartners = value; } }
        public string CarriersMarketing { get { return this.carriersMarketing; } set { this.carriersMarketing = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<MOBSHOPSearchFilterItem> CarriersMarketingList { get { return this.carriersMarketingList; } set { this.carriersMarketingList = value; } }
        public string CarriersOperating { get { return this.carriersOperating; } set { this.carriersOperating = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<MOBSHOPSearchFilterItem> CarriersOperatingList { get { return this.carriersOperatingList; } set { this.carriersOperatingList = value; } }
        public bool CarrierStar { get { return this.carrierStar; } set { this.carrierStar = value; } }
        public int DurationMax { get { return this.durationMax; } set { this.durationMax = value; } }
        public int DurationMin { get { return this.durationMin; } set { this.durationMin = value; } }
        public int DurationStopMax { get { return this.durationStopMax; } set { this.durationStopMax = value; } }
        public int DurationStopMin { get { return this.durationStopMin; } set { this.durationStopMin = value; } }
        public string EquipmentCodes { get { return this.equipmentCodes; } set { this.equipmentCodes = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<MOBSHOPSearchFilterItem> EquipmentList { get { return this.equipmentList; } set { this.equipmentList = value; } }
        public string EquipmentTypes { get { return this.equipmentTypes; } set { this.equipmentTypes = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<MOBSHOPFareFamily> FareFamilies { get { return this.fareFamilies; } set { this.fareFamilies = value; } }
        public string FareFamily { get { return this.fareFamily; } set { this.fareFamily = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public decimal PriceMax { get { return this.priceMax; } set { this.priceMax = value; } }
        public decimal PriceMin { get { return this.priceMin; } set { this.priceMin = value; } }
        public string PriceMaxDisplayValue { get { return this.priceMaxDisplayValue; } set { this.priceMaxDisplayValue = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string PriceMinDisplayValue { get { return this.priceMinDisplayValue; } set { this.priceMinDisplayValue = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public int StopCountExcl { get { return this.stopCountExcl; } set { this.stopCountExcl = value; } }
        public int StopCountMax { get { return this.stopCountMax; } set { this.stopCountMax = value; } }
        public int StopCountMin { get { return this.stopCountMin; } set { this.stopCountMin = value; } }
        public string TimeArrivalMax { get { return this.timeArrivalMax; } set { this.timeArrivalMax = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string TimeArrivalMin { get { return this.timeArrivalMin; } set { this.timeArrivalMin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string TimeDepartMax { get { return this.timeDepartMax; } set { this.timeDepartMax = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string TimeDepartMin { get { return this.timeDepartMin; } set { this.timeDepartMin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<string> Warnings { get { return this.warnings; } set { this.warnings = value; } }
        public List<MOBSHOPSearchFilterItem> WarningsFilter { get { return this.warningsFilter; } set { this.warningsFilter = value; } }
        public int PageNumber { get { return this.pageNumber; } set { this.pageNumber = value; } }
        public string SortType1 { get { return this.sortType1; } set { this.sortType1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<MOBSHOPSearchFilterItem> SortTypes { get { return this.sortTypes; } set { this.sortTypes = value; } }
        public List<MOBSHOPSearchFilterItem> NumberofStops { get { return this.numberofStops; } set { this.numberofStops = value; } }
        public List<MOBSHOPSearchFilterItem> AmenityTypes { get { return this.amenityTypes; } set { this.amenityTypes = value; } }
        public List<MOBSHOPSearchFilterItem> AircraftCabinTypes { get { return this.aircraftCabinTypes; } set { this.aircraftCabinTypes = value; } }
        public List<MOBSHOPSearchFilterItem> CarrierTypes { get { return this.carrierTypes; } set { this.carrierTypes = value; } }
        public bool ShowPriceFilters { get { return this.showPriceFilters; } set { this.showPriceFilters = value; } }
        public bool ShowDepartureFilters { get { return this.showDepartureFilters; } set { this.showDepartureFilters = value; } }
        public bool ShowArrivalFilters { get { return this.showArrivalFilters; } set { this.showArrivalFilters = value; } }
        public bool ShowDurationFilters { get { return this.showDurationFilters; } set { this.showDurationFilters = value; } }
        public bool ShowLayOverFilters { get { return this.showLayOverFilters; } set { this.showLayOverFilters = value; } }
        public bool ShowSortingandFilters { get { return this.showSortingandFilters; } set { this.showSortingandFilters = value; } }
        public bool FilterSortPaging { get { return this.filterSortPaging; } set { this.filterSortPaging = value; } }
        public string MaxArrivalDate { get { return this.maxArrivalDate; } set { this.maxArrivalDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string MinArrivalDate { get { return this.minArrivalDate; } set { this.minArrivalDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public bool ShowRefundableFaresToggle { get { return this.showRefundableFaresToggle; } set { this.showRefundableFaresToggle = value; } }
        public MOBSHOPSearchFilterItem RefundableFaresToggle {  get { return this.refundableFaresToggle;  } set { this.refundableFaresToggle = value; } }
        public List<MOBSHOPSearchFilterItem> AdditionalToggles { get { return this.additionalToggles; } set { this.additionalToggles = value; } }

    }
}
