using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Internal.CompleteBooking;

namespace United.Mobile.Model.Internal.CancelReservation
{
    public class EresPNRRetrievalResponse : EResBaseResponse
    {
        public PNRRetrievalBooking PNR { get; set; }
        public string OffsetCabinUrl { get; set; } 
        public string HotelWidgetURL { get; set; } 
        public string ETicketedMessage { get; set; }
        public string AddtionalRequestMessage { get; set; }
        public string BookingType { get; set; } 
        public bool IsUMNRTravelling { get; set; }
        public List<UnaccompaniedMinorAdultContactPerson> AdultContacts { get; set; } 
        public bool IsDeadHeadBooking { get; set; } 
        public bool IsPostiveSpaceBooking { get; set; }
        public string ConfirmationID { get; set; } 
        public bool ShowRequestReciept { get; set; }
        public bool IsPaymentByPrepaid { get; set; } 
        public bool IsAuthorizedTravelTypePNR { get; set; }
        public string EstimatedTaxMessage { get; set; } 
        public string SelectSeatURL { get; set; }
        public bool ShowCheckIn { get; set; }
        public string CheckInUrl { get; set; } 
        public string PnrCreationDate { get; set; } 
        public bool IsPartiallyUsedPNR { get; set; } 
    }
}

