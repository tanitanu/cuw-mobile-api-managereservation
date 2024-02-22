using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
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
