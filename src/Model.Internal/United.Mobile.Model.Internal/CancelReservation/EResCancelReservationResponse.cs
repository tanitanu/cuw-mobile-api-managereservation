using System.Collections.Generic;
using United.Mobile.Model.Internal.Booking;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.CancelReservation
{
    public class EResCancelReservationResponse : EResBaseResponse
    {
        public bool IsCancelled { get; set; }
        public List<EResAlert> CancelMsg { get; set; }
        public List<PNRSegment> Segments { get; set; }
        public List<PNRPassRider> Passenger { get; set; }
        public string RequestRefundURL { get; set; }
        public string RefundMessgae { get; set; }
        public string HotelWidgetURL { get; set; }
    }
}
