using System;
using System.Collections.Generic;
using United.Mobile.Model.Shopping;

namespace United.Persist.Definition.Shopping
{
    [Serializable()]
    public class MOBTrip : MOBTripBase
    {
        public string tripId { get; set; } = string.Empty;
        public string originDecoded { get; set; } = string.Empty;
        public string destinationDecoded { get; set; } = string.Empty;
        public int flightCount;
        public int totalFlightCount;
        public List<MOBSHOPFlattenedFlight> FlattenedFlights { get; set; }
        public List<MOBSHOPFlightSection> flightSections { get; set; }
        public int lastTripIndexRequested;
        public List<MOBSHOPShoppingProduct> columns { get; set; }
        public string yqyrMessage { get; set; } = string.Empty;
        public int pageCount;
        public string originDecodedWithCountry { get; set; } = string.Empty;
        public string destinationDecodedWithCountry { get; set; } = string.Empty;
        public bool showOriginDestinationForFlights;
        public bool disableEplus;
    }
}
