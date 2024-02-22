using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.UpgradeCabin
{
    [Serializable()]
    public class UpgradeCabinSegment : Segment
    {
        private string tripNumber;
        private string segmentNumber;
        private string classType;
        private string classOfService;
        private string classOfServiceDescription;
        private string operatingAirlineCode;
        private string operatingAirlineFlightNumber;
        private string operatingAirlineName;
        private string waitlisted;
        private Boolean isPlusPointsExpiredBeforeTravel;
        private Boolean showPlusPointsExpiryMessage;
        private Aircraft aircraft;
        private string flightTime;
        private List<UpgradeOption> prices;
        private List<UpgradeOption> points;
        private List<UpgradeOption> miles;

        public string TripNumber { get { return this.tripNumber; } set { this.tripNumber = value; } }
        public string SegmentNumber { get { return this.segmentNumber; } set { this.segmentNumber = value; } }
        public string ClassType { get { return this.classType; } set { this.classType = value; } }
        public string ClassOfService { get { return this.classOfService; } set { this.classOfService = value; } }
        public string ClassOfServiceDescription { get { return this.classOfServiceDescription; } set { this.classOfServiceDescription = value; } }
        public string OperatingAirlineCode { get { return this.operatingAirlineCode; } set { this.operatingAirlineCode = value; } }
        public string OperatingAirlineFlightNumber { get { return this.operatingAirlineFlightNumber; } set { this.operatingAirlineFlightNumber = value; } }
        public string OperatingAirlineName { get { return this.operatingAirlineName; } set { this.operatingAirlineName = value; } }
        public string Waitlisted { get { return this.waitlisted; } set { this.waitlisted = value; } }
        public Boolean IsPlusPointsExpiredBeforeTravel { get { return this.isPlusPointsExpiredBeforeTravel; } set { this.isPlusPointsExpiredBeforeTravel = value; } }
        public Boolean ShowPlusPointsExpiryMessage { get { return this.showPlusPointsExpiryMessage; } set { this.showPlusPointsExpiryMessage = value; } }
        public Aircraft Aircraft { get { return this.aircraft; } set { this.aircraft = value; } }
        public string FlightTime { get { return this.flightTime; } set { this.flightTime = value; } }
        public List<UpgradeOption> Prices { get { return this.prices; } set { this.prices = value; } }
        public List<UpgradeOption> Points { get { return this.points; } set { this.points = value; } }
        public List<UpgradeOption> Miles { get { return this.miles; } set { this.miles = value; } }
    }
}
