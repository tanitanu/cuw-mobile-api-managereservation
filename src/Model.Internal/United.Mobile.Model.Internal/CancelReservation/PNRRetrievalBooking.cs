using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Internal.CompleteBooking;

namespace United.Mobile.Model.Internal.CancelReservation
{
    public class PNRRetrievalBooking
    {
        public List<BookingPNRSegment> Segments { get; set; }
        public List<PNRRetrievalBookingPassenger> Passengers { get; set; }
        public string TravelPlan { get; set; }
        public string ConfirmationMsg { get; set; }
        public string EmployeeId { get; set; }
        public BookingPnrSettings BookingPNRSettings { get; set; }
        public TypeOption ClassOfService { get; set; }
        public EmpTCDInfo HomePhone { get; set; }
        public EmpTCDInfo WorkPhone { get; set; }
        public string DeliveryType { get; set; }
        public string DeliveryValue { get; set; }
        public List<string> RemarkList { get; set; }
        public bool PreventTicketing { get; set; }
        public bool ETicketIndicator { get; set; }
        public string TotalCost { get; set; }
        public PassType PassType { get; set; }
        public List<EResAlert> DisplayError { get; set; }
        public int TotalBuddyPasses { get; set; }
        public int TotalVacationPasses { get; set; }
        public int TotalFamilyVacationPasses { get; set; }
        public int TotalPersonalPasses { get; set; }
        public string PassDeductedMessage { get; set; }
        public bool IsPaymentFailure { get; set; }
        public List<string> Errors { get; set; }

    }
}
