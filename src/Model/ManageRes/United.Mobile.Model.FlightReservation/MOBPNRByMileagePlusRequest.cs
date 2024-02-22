using System;
using United.Mobile.Model.Common;
using United.Mobile.Model.Fitbit;

namespace United.Mobile.Model.FlightReservation
{
    [Serializable()]
    public class MOBPNRByMileagePlusRequest : MOBRequest
    {
        private string mileagePlusNumber = string.Empty;

        public MOBPNRByMileagePlusRequest()
            : base()
        {
        }

        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public MOBReservationType ReservationType { get; set; }
    }
}
