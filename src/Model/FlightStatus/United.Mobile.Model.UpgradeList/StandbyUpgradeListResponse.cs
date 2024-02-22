namespace United.Mobile.Model.UpgradeList
{
    public class StandbyUpgradeListResponse
    {
        public bool isFound { get; set; }
        public Data data { get; set; }
        public object[] errors { get; set; }
    }

    public class Data
    {
        public StandbySegment segment { get; set; }
        public StandbyPbt[] pbts { get; set; }
        public Checkinsummary[] checkInSummaries { get; set; }
        //public int numberOfCabins { get; set; }
        public string numberOfCabins { get; set; }
        public Front front { get; set; }
        public Middle middle { get; set; }
        public Rear rear { get; set; }
        public float executionTimeInMilliseconds { get; set; }
    }

    public class StandbySegment
    {
        public string airlineCode { get; set; }
        public int flightNumber { get; set; }
        public int codeShareFlightNumber { get; set; }
        public string flightDate { get; set; }
        public string departureAirportCode { get; set; }
        public string departureAirportName { get; set; }
        public string arrivalAirportCode { get; set; }
        public string arrivalAirportName { get; set; }
        public string scheduledDepartureTime { get; set; }
        public string scheduledArrivalTime { get; set; }
        public string ship { get; set; }
        public string equipmentCode { get; set; }
        public string equipmentDescription { get; set; }
        public string equipmentDescriptionLong { get; set; }
        public bool departed { get; set; }
    }

    public class Front
    {
        public Standby[] cleared { get; set; }
        public Standby[] standby { get; set; }
    }

    public class Standby
    {
        public string recordLocator { get; set; }
        public string standbyCabin { get; set; }
        public string currentCabin { get; set; }
        public string bookedCabin { get; set; }
        public int checkinId { get; set; }
        public bool isCheckedIn { get; set; }
        public string classOfService { get; set; }
        public string originalClassOfService { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string passengerName { get; set; }
        public int numberInParties { get; set; }
        public string fqtvNumber { get; set; }
        public string standbyPassCode { get; set; }
        public string premierLevel { get; set; }
        public string evaluatedRank { get; set; }
        public string seatNumber { get; set; }
        public bool skipped { get; set; }
        public string skippedReason { get; set; }
        public string upgradeClassOfService { get; set; }
        public string upgradeType { get; set; }
        public string clearanceType { get; set; }
        //  public DateTime cabinClearanceTime { get; set; }
        public bool canBeClearedIntoCabin { get; set; }
        public string canBeClearedIntoCabinStartingAt { get; set; }
    }

    public class Middle
    {
        public Standby[] cleared { get; set; }
        public Standby[] standby { get; set; }
    }



    public class Rear
    {
        public Standby[] cleared { get; set; }
        public Standby[] standby { get; set; }
    }


    public class StandbyPbt
    {
        public string cabin { get; set; }
        public int capacity { get; set; }
        public int authorized { get; set; }
        public int booked { get; set; }
        public int held { get; set; }
        public int reserved { get; set; }
        public int revenueStandby { get; set; }
        public int waitList { get; set; }
        public int jump { get; set; }
        public int group { get; set; }
        public int ps { get; set; }
        public int sa { get; set; }
    }

    public class Checkinsummary
    {
        public string cabin { get; set; }
        public int capacity { get; set; }
        public int through { get; set; }
        public int localNonEtktBoarding { get; set; }
        public int localEtktBoarding { get; set; }
        public int connectingNonEtkt { get; set; }
        public int connectingEtkt { get; set; }
        public int standbyNonEtktWithSeats { get; set; }
        public int standbyEtktWithSeats { get; set; }
        public int total { get; set; }
        public int etktPassengersCheckedIn { get; set; }
        public int heldAsaAciSeats { get; set; }
        public int revStandbyCheckedInWithoutSeats { get; set; }
        public int nonRevStandbyCheckedInWithoutSeats { get; set; }
        public int children { get; set; }
        public int infants { get; set; }
        public int bags { get; set; }
    }
}
