using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Internal.Common
{

    [System.Serializable]
    public class Reservation
    {
        public List<ReservationSegment> Segments { get; set; }
    }
    public class ReservationSegment
    {
        public List<Passenger> Passengers { get; set; }

    }
    public class Passenger
    {
        public PassengerSegmentInformation PassengerSegmentInformation { get; set; }
    }
    public class PassengerSegmentInformation
    {
        public List<BaggageInformation> BaggageInformation { get; set; }
    }
    public class BaggageInformation
    {
        public string BagTagNumber { get; set; }
        public string BagTagUniqueKey { get; set; }
    }
    public class CacheRequest
    {
        public string TransactionId { get; set; }
        public string Key { get; set; }
        public string Bucket { get; set; }
    }
}
