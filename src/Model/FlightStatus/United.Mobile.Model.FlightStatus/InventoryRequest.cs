namespace United.Mobile.Model.FlightStatus
{
    public class InventoryRequest
    {
        public string AccessCode { get; set; }
        
        public string Date { get; set; }
        
        public short Days { get; set; }
        
        public string Destination { get; set; }
        
        public bool DisableFlifo { get; set; }

        public short DurationSlack { get; set; }

        public short DurationMultiplier { get; set; }

        public short DurationPenalty { get; set; }

        public bool DurationRuleOn { get; set; } 

        public bool ExcludeZeroAvailabilityTripsOn { get; set; }

        public string FlightNumber { get; set; } = string.Empty;

        public bool IncludeCancelledFlights { get; set; }
        
        public bool IncludeOAMainFlights { get; set; }
        
        public bool IncludeStarMainFlights { get; set; }
        
        public bool IncludeUACodeShareFlights { get; set; }
        
        public bool IncludeUAMainFlights { get; set; }
        
        public bool IncludeUARegionalFlights { get; set; }
        
        public bool IncludeStarCodeShareFlights { get; set; }

        public string MarketingCarriersInclusive { get; set; } = string.Empty;

        public string MarketingCarriersExclusive { get; set; } = string.Empty;

        public short MaxTrips { get; set; }

        public bool MaxMileRuleOn { get; set; }

        public short MaxMiles { get; set; }

        public bool MaxJourneyRuleOn { get; set; }

        public short MaxJourney { get; set; }

        public string MidPointsInclusive { get; set; } = string.Empty;

        public string MidPointsExclusive { get; set; } = string.Empty;

        public bool IncludeDepartedFlights { get; set; }
        
        public int MaxConnectTimeMinutes { get; set; }
        
        public bool MaxConnectTimeRuleOn { get; set; }
        
        public int MinConnectTimeMinutes { get; set; }
        
        public int NBA_Destination { get; set; }
        
        public int NBA_Origin { get; set; }
        
        public string Origin { get; set; }

        public string OperatingCarriersInclusive { get; set; } = string.Empty;

        public string OperatingCarriersExclusive { get; set; } = string.Empty;

        public string ResponseFormat { get; set; }

        public string Routes { get; set; } = string.Empty;

        public string RequiredAvailabilitySumAllFlights { get; set; } = string.Empty;

        public int Stops { get; set; }
        
        public bool StopsInclusive { get; set; }
        
        public int Time { get; set; }
        
        public bool TrueAvailabilityOn { get; set; }
    }
}