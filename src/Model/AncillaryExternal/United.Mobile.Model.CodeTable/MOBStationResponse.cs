using System;
using System.Collections.Generic;

namespace United.Mobile.Model.CodeTable
{
    [Serializable()]
    public class MOBStationResponse : MOBResponse
    {
        
        public string AvailableFlag { get; set; } = string.Empty;

        public List<MOBStation> Stations { get; set; }
        public MOBStationResponse()
        {
            Stations = new List<MOBStation>();
        }

    }
}
