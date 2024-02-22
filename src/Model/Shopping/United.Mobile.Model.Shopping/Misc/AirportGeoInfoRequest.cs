using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
{
    public class AirportGeoInfoRequest : MOBRequest
    {
        public string Latitude { get; set; } = string.Empty;

        public string Longitude { get; set; } = string.Empty;

        public string Radius { get; set; } = string.Empty;
    }
}

