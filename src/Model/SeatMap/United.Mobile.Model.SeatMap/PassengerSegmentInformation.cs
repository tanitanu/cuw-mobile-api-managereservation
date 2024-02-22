
namespace United.Mobile.Model.SeatMap
{
    [System.Serializable]
    public class PassengerSegmentInformation
    {
        public string SeatNumber { get; set; }
        public BookingClass BookingClass { get; set; }
        public bool IsEmployee { get; set; }
        public bool HasLapInfant { get; set; }
        public bool IsChild { get; set; }
        public bool IsCheckInEligible { get; set; }
        public bool? IsCheckInEligibleNullableBool { get; set; }
        public bool IsCheckedIn { get; set; }
        public bool IsBoardingNow { get; set; }
        public bool IsBoarded { get; set; }
        public bool IsUsed { get; set; }
        public bool IsIrrOp { get; set; }
        public bool IrrOpsViewed { get; set; }
        public string BookingClub { get; set; }
        public bool IsStandBy { get; set; }
        public string InformationDocument { get; set; }
        public bool isDeboarded { get; set; }
        public bool isNRSASegment { get; set; }
        public string TicketProductCode { get; set; }
        public string CabinCode { get; set; }
        public string InfoDocBarCode { get; set; }
        public string PriorityCode { get; set; }
    }
}
