namespace United.Mobile.Model.FlightSearchResult
{
    public class SegmentPBT
    {
        public PBTType CheckedIn { get; set; }
        public PBTType UpgradableElite { get; set; }
        public PBTType Capacity { get; set; }
        public PBTType Available { get; set; }
        public PBTType Authorized { get; set; }
        public PBTType Booked { get; set; }
        public PBTType RevenueStandBy { get; set; }
        public PBTType Held { get; set; }
        public PBTType PositiveSpace { get; set; }
        public PBTType SpaceAvailable { get; set; }
        public PBTType Group { get; set; }
    }
}