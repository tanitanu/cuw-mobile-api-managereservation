using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class AirportGeoInfoResponse :MOBResponse
    {
        public AirportGeoInfoResponse()
            : base()
        {
        }

        private AirportGeoInfo airportGeoInfo;

        public AirportGeoInfo AirportGeoInfo
        {
            get
            {
                return this.airportGeoInfo;
            }
            set
            {
                this.airportGeoInfo = value;
            }
        }
    }
}
