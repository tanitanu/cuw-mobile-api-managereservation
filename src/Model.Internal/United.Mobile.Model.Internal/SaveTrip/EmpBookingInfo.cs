using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.SaveTrip
{
    public class EmpBookingInfo 
    {
        public string SessionId { get; set; }
        public string BookingConfirmationNumber { get; set; }
        public int NumberOfTrips { get; set; }
        public List<BookingTrip> BookingTrips { get; set; }
        public List<EmpBookingPassenger> BookingPassengers { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string DeliveryType { get; set; }
        public string DeliveryValue { get; set; }
        public EmpName Name { get; set; }
        public string CardType { get; set; }
        public string CardTypeName { get; set; }
        public string CardNumber { get; set; }
        public string CID { get; set; }
        public string ExpMonth { get; set; }
        public string ExpYear { get; set; }
        public string PassType { get; set; }
        public string QualifiedEmergency { get; set; }
        public string EmergencyNature { get; set; }
        public bool PayByCreditCard { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsChangeSegment { get; set; }
        public EmpTravelingWith TravelingWith { get; set; }
    }

}
