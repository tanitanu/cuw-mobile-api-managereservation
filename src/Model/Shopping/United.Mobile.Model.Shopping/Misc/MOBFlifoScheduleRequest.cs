using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlifoScheduleRequest : MOBRequest
    {
        public FlifoScheduleRequest(string Origin, string Destination, string Date,
                               short Days, string FlightType)
        {
            this.Origin = Origin;
            this.Destination = Destination;
            this.Date = Date;
            this.Days = Days;
            this.FlightType = FlightType;
        }

        public FlifoScheduleRequest()
            : base()
        {
        }


        private string origin = string.Empty;
        private string destination = string.Empty;
        private string date = string.Empty;
        private short days;
        private string flightType = string.Empty;
        private string carrierCode = string.Empty; //Nizam - #243511(OA Flifo) - Included carriers 


        public string Origin
        {
            get { return this.origin; }
            set
            {
                this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Destination
        {
            get { return this.destination; }
            set
            {
                this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Date
        {
            get { return this.date; }
            set
            {
                this.date = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public short Days
        {
            get
            {
                return days;
            }
            set
            {
                days = value;
            }
        }

        public string FlightType
        {
            get
            {
                return this.flightType;
            }
            set
            {
                this.flightType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        //Nizam - #243511(OA Flifo) - Included carriers 
        public string CarrierCode
        {
            get
            {
                return this.carrierCode;
            }
            set
            {
                this.carrierCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
