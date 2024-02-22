using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlifoScheduleEngineRequest
    {
        public string AccessCode { get; set; }
        
        public string ResponseFormat = "xml";
        
        public string Origin { get; set; }
        
        public string Destination { get; set; }
        
        public string Date { get; set; }
        
        public short Time { get; set; }
        
        public short Stops { get; set; }
        
        public bool StopsInclusive = true;
        
        public short Days = 1;

        public short MaxTrips { get; set; }
        
        public bool MaxMileRuleOn = true;

        public short MaxMiles { get; set; }
        
        public bool MaxJourneyRuleOn = true;

        public short MaxJourney { get; set; }
        
        public bool DurationRuleOn = true;

        public short DurationMultiplier { get; set; }
        
        public short DurationSlack = 60;
        
        public short DurationPenalty = 90;
        
        public short NBA_Origin = 0;
        
        public short NBA_Destination = 0;
        
        public string MarketingCarriersInclusive = string.Empty;
        
        public string MarketingCarriersExclusive = string.Empty;
        
        public string OperatingCarriersInclusive = string.Empty;
        
        public string OperatingCarriersExclusive = string.Empty;
        
        public string MidPointsInclusive = string.Empty;
        
        public string MidPointsExclusive = string.Empty;
        
        public string Routes = string.Empty;
        
        public bool DisableFlifo = false;
        
        public int MinConnectTimeMinutes = 0;
        
        public int MaxConnectTimeMinutes = 0;
        
        public bool MaxConnectTimeRuleOn = true;
        
        public bool IncludeUAMainFlights = true;
        
        public bool IncludeUARegionalFlights = true;
        
        public bool IncludeUACodeShareFlights = true;
        
        public bool IncludeStarMainFlights = true;
        
        public bool IncludeOAMainFlights = true;
        
        public bool IncludeCancelledFlights = false;
        
        public string FlightNumber = string.Empty;
        
        public bool ExcludeZeroAvailabilityTripsOn = false;
        
        public bool TrueAvailabilityOn = false;
        
        public string RequiredAvailabilitySumAllFlights = string.Empty;
    }
}
