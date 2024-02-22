using System;
using System.Runtime.Serialization;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBSHOPTripBase
    {
        private string origin = string.Empty;

        //MB-2639 add all airports flag
        private int originAllAirports = -1;

        private string destination = string.Empty;

        //MB-2639 add all airports flag
        private int destinationAllAirports = -1;

        private string departDate = string.Empty;
        private string arrivalDate = string.Empty;
        private string cabin = string.Empty;
        private bool useFilters = false;
        private MOBSHOPSearchFilters searchFiltersIn;
        private MOBSHOPSearchFilters searchFiltersOut;

        private bool searchNearbyOriginAirports = false;
        private bool searchNearbyDestinationAirports = false;
        private string shareMessage = string.Empty;
        private int index;


        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }
        public string Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public int OriginAllAirports
        {
            get
            {
                return originAllAirports;
            }
            set
            {
                this.originAllAirports = value;
            }
        }

        public string Destination
        {
            get
            {
                return this.destination;
            }
            set
            {
                this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
        public int DestinationAllAirports
        {
            get
            {
                return destinationAllAirports;
            }
            set
            {
                this.destinationAllAirports = value;
            }
        }

        public string DepartDate
        {
            get
            {
                return this.departDate;
            }
            set
            {
                this.departDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ArrivalDate
        {
            get
            {
                return this.arrivalDate;
            }
            set
            {
                this.arrivalDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Cabin
        {
            get
            {
                return this.cabin;
            }
            set
            {
                this.cabin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool UseFilters
        {
            get
            {
                return this.useFilters;
            }
            set
            {
                this.useFilters = value;
            }
        }

        public MOBSHOPSearchFilters SearchFiltersIn
        {
            get
            {
                return this.searchFiltersIn;
            }
            set
            {
                this.searchFiltersIn = value;
            }
        }

        public MOBSHOPSearchFilters SearchFiltersOut
        {
            get
            {
                return this.searchFiltersOut;
            }
            set
            {
                this.searchFiltersOut = value;
            }
        }

        public bool SearchNearbyOriginAirports
        {
            get
            {
                return this.searchNearbyOriginAirports;
            }
            set
            {
                this.searchNearbyOriginAirports = value;
            }
        }

        public bool SearchNearbyDestinationAirports
        {
            get
            {
                return this.searchNearbyDestinationAirports;
            }
            set
            {
                this.searchNearbyDestinationAirports = value;
            }
        }
        public string ShareMessage
        {
            get
            {
                return this.shareMessage;
            }
            set
            {
                this.shareMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private MOBSHOPTripChangeType changeType = MOBSHOPTripChangeType.NoChange;
        public MOBSHOPTripChangeType ChangeType
        {
            get
            {
                return this.changeType;
            }
            set
            {
                this.changeType = value;
            }
        }
    }

    [Serializable]
    public enum MOBSHOPTripChangeType
    {
        [EnumMember(Value = "0")]
        ChangeFlight,
        [EnumMember(Value = "1")]
        AddFlight,
        [EnumMember(Value = "2")]
        DeleteFlight,
        [EnumMember(Value = "3")]
        NoChange
    }
}

//Example Search Filters
/*
 <SearchFiltersOut AircraftTypes="Jet" AirportsOrigin="IAH" AirportsDestination="LAS" AirportsStop="DEN,LAX,ORD,SFO" CabinCountMin="2" CabinCountMax="2" CarrierDefault="true" CarrierExpress="true" CarrierPartners="true" CarrierStar="true" CarriersMarketing="UA" CarriersOperating="UA" DurationMin="187" DurationMax="568" DurationStopMin="-1" DurationStopMax="-1" EquipmentCodes="319,320,738,739,752,753,788,CR7" EquipmentTypes="Jet" PriceMin="358.50" PriceMax="1658.49" StopCountExcl="-1" StopCountMin="0" StopCountMax="1" TimeDepartMin="06:00" TimeDepartMax="21:29" TimeArrivalMin="00:15" TimeArrivalMax="23:21">
- <AirportsDestinationList>
<CodeDescPair Code="LAS" Description="Las Vegas, NV (LAS)" /> 
</AirportsDestinationList>
- <AirportsOriginList>
<CodeDescPair Code="IAH" Description="Houston, TX (IAH - Intercontinental)" /> 
</AirportsOriginList>
- <AirportsStopList>
<CodeDescPair Code="DEN" Description="Denver, CO (DEN)" /> 
<CodeDescPair Code="LAX" Description="Los Angeles, CA (LAX)" /> 
<CodeDescPair Code="ORD" Description="Chicago, IL (ORD - O'Hare)" /> 
<CodeDescPair Code="SFO" Description="San Francisco, CA (SFO)" /> 
</AirportsStopList>
<AirportsStopToAvoidList /> 
- <CarriersMarketingList>
<CodeDescPair Code="UA" Description="United Airlines" /> 
</CarriersMarketingList>
- <CarriersOperatingList>
<CodeDescPair Code="UA" Description="United Airlines" /> 
</CarriersOperatingList>
- <EquipmentList>
<CodeDescPair Code="319" Description="Airbus A319" /> 
<CodeDescPair Code="320" Description="Airbus A320" /> 
<CodeDescPair Code="738" Description="Boeing 737-800" /> 
<CodeDescPair Code="739" Description="Boeing 737-900" /> 
<CodeDescPair Code="752" Description="Boeing 757-200" /> 
<CodeDescPair Code="753" Description="Boeing 757-300" /> 
<CodeDescPair Code="788" Description="Boeing 787-8 Dreamliner" /> 
<CodeDescPair Code="CR7" Description="Canadair Regional Jet 700" /> 
</EquipmentList>
- <FareFamilies>
<fareFamily fareFamily="ECONOMY" maxMileage="867" maxPrice="USD951.50" minMileage="325" minPrice="USD358.50" minPriceInSummary="true" /> 
<fareFamily fareFamily="MIN-BUSINESS-OR-FIRST" maxMileage="1524" maxPrice="USD1658.49" minMileage="464" minPrice="USD507.49" minPriceInSummary="false" /> 
</FareFamilies>
- <Warnings>
<string>CHANGE_OF_TERMINAL</string> 
<string>LONG_LAYOVER</string> 
</Warnings>
</SearchFiltersOut>
*/

