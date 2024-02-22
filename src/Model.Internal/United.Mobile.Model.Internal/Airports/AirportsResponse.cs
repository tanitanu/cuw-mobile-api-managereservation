using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.Airports
{
    public class AirportsResponse: EResBaseResponse
    {
        public List<AirportSearch> AirportSearch { get; set; } = new List<AirportSearch>();
        public string Message { get; set; }
    }
}

