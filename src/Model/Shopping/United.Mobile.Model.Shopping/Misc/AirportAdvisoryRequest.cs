using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class AirportAdvisoryRequest : MOBRequest
    {
        private string segments = string.Empty;
        private string flightDate = string.Empty;

        public string Segments
        {
            get
            {
                return this.segments;
            }
            set
            {
                this.segments = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string FlightDate
        {
            get
            {
                return this.flightDate;
            }
            set
            {
                this.flightDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
