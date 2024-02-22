using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBEmpBookingTrip
    {
        private List<MOBEmpBookingSegment> bookingSegment;

        public List<MOBEmpBookingSegment> BookingSegment
        {
            get { return bookingSegment; }
            set { bookingSegment = value; }
        }

    }
}
