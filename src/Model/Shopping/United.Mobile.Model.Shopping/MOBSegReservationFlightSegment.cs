using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBSegReservationFlightSegment
    {
        private ComBookingClass bookingClass;
        private SegFlightSegment flightSegment;
        private string isConnection = string.Empty;
        private string otherAirlineRecordLocator = string.Empty;

        public ComBookingClass BookingClass
        {
            get
            {
                return this.bookingClass;
            }
            set
            {
                this.bookingClass = value;
            }
        }

        public SegFlightSegment FlightSegment
        {
            get
            {
                return this.flightSegment;
            }
            set
            {
                this.flightSegment = value;
            }
        }

        public string IsConnection
        {
            get
            {
                return this.isConnection;
            }
            set
            {
                this.isConnection = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OtherAirlineRecordLocator
        {
            get
            {
                return this.otherAirlineRecordLocator;
            }
            set
            {
                this.otherAirlineRecordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
