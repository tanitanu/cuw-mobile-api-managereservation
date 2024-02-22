using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    public class ReservationSegment : Segment
    {
        public int Stops { get; set; }
        public int SegmentNumber { get; set; }
        public int TripNumber { get; set; }
        public string RecordLocator { get; set; }
        public string BoardingBeginDateTime { get; set; }
        public string BoardingBeginDateTimeUtc { get; set; }
        public string BoardingEndDateTime { get; set; }
        public List<Passenger> Passengers { get; set; } = new List<Passenger>();
        public bool HasUnitedClubAtDeparture { get; set; }
        public bool HasUnitedClubAtArrival { get; set; }
        public BookingClass BookingClass { get; set; }
        public int ScheduledFlightTime { get; set; }
        public bool IsLastSegmentInTrip { get; set; }
        public List<SSR> SSRs { get; set; }
        public bool IsTouchLessEnable { get; set; }
        public bool HasSavedSeatPref { get; set; } = false;
        public bool enableSaveButton { get; set; } = false;
    }
}
