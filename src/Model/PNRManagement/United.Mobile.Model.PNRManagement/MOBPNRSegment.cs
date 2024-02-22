using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.MP2015;
using MOBScheduleChange = United.Mobile.Model.ManagRes.MOBScheduleChange;

namespace United.Mobile.Model.PNRManagement
{
    [Serializable()]
    public class MOBPNRSegment : MOBSegment
    {
        private MOBAirline operationoperatingCarrier;
        private MOBAircraft aircraft;
        private string meal = string.Empty;
        private string flightTime = string.Empty;
        private string groundTime = string.Empty;
        private string totalTravelTime = string.Empty;
        private string actualMileage = string.Empty;
        private string mileagePlusMileage = string.Empty;
        private string emp = string.Empty;
        private string totalMileagePlusMileage = string.Empty;
        private string ssrMeals = string.Empty;
        private string classOfService = string.Empty;
        private string classOfServiceDescription = string.Empty;
        private List<MOBPNRSeat> seats = new List<MOBPNRSeat>();
        private string numberOfStops = string.Empty;
        private List<MOBPNRSegment> stops = new List<MOBPNRSegment>();
        private bool isPlusPointSegment;
        private string scheduledDepartureDateTimeGMT = string.Empty;
        private string scheduledArrivalDateTimeGMT = string.Empty;

        private MOBAirline codeshareCarrier = new MOBAirline();
        private string codeshareFlightNumber = string.Empty;
        private List<MOBScheduleChange> scheduleChangeInfo;
        private MOBSegmentResponse upgradeVisibility;
        private int lowestEliteLevel;
        private bool upgradeable;
        private string upgradeableMessageCode = string.Empty;
        private string upgradeableMessage = string.Empty;
        private string upgradeableRemark = string.Empty;
        private string complimentaryInstantUpgradeMessage = string.Empty;

        private bool remove;
        private bool waitlisted;
        private string actionCode = string.Empty;

        private bool upgradeEligible;

        private string waitedCOSDesc = string.Empty;

        private List<LmxProduct> lmxProducts;

        private string cabinType = string.Empty;

        private bool nonPartnerFlight = false;

        private List<MOBBundle> bundles;

        private bool isElf;
        private bool isIBE;
        private string tripNumber;
        private int segmentNumber;
        private string ticketCouponStatus;
        private string productCode;
        private string fareBasisCode;

        private MOBInCabinPet inCabinPetInfo;
        private bool showSeatMapLink;

        private bool isChangeOfGuage;
        private bool isCheckedIn;
        private bool isCheckInEligible;
        private bool isAllPaxCheckedIn;

        public bool IsCheckedIn { get { return isCheckedIn; } set { isCheckedIn = value; } }
        public bool IsCheckInEligible { get { return isCheckInEligible; } set { isCheckInEligible = value; } }
        public bool IsAllPaxCheckedIn { get { return isAllPaxCheckedIn; } set { isAllPaxCheckedIn = value; } }


        public List<MOBScheduleChange> ScheduleChangeInfo
        {
            get { return scheduleChangeInfo; }
            set { scheduleChangeInfo = value; }
        }
        public bool IsChangeOfGuage
        {
            get { return isChangeOfGuage; }
            set { isChangeOfGuage = value; }
        }

        public bool ShowSeatMapLink
        {
            get { return showSeatMapLink; }
            set { showSeatMapLink = value; }
        }

        public MOBInCabinPet InCabinPetInfo
        {
            get { return this.inCabinPetInfo; }
            set { this.inCabinPetInfo = value; }

        }

        public string SSRMeals
        {
            get { return this.ssrMeals; }
            set { this.ssrMeals = value; }

        }
        public string TicketCouponStatus
        {
            get { return ticketCouponStatus; }
            set { ticketCouponStatus = value; }
        }

        public string TripNumber
        {
            get { return tripNumber; }
            set { tripNumber = value; }
        }
        public int SegmentNumber
        {
            get { return segmentNumber; }
            set { segmentNumber = value; }
        }


        public MOBPNRSegment()
            : base()
        {
        }

        public bool IsPlusPointSegment { get { return this.isPlusPointSegment; } set { this.isPlusPointSegment = value; } }
        public bool IsElf
        {
            get { return this.isElf; }
            set { this.isElf = value; }
        }

        public bool IsIBE
        {
            get { return isIBE; }
            set { isIBE = value; }
        }

        public List<MOBBundle> Bundles
        {
            get
            {
                return this.bundles;
            }
            set
            {
                this.bundles = value;
            }
        }

        public MOBAirline OperationoperatingCarrier
        {
            get
            {
                return this.operationoperatingCarrier;
            }
            set
            {
                this.operationoperatingCarrier = value;
            }
        }

        public MOBAircraft Aircraft
        {
            get
            {
                return this.aircraft;
            }
            set
            {
                this.aircraft = value;
            }
        }

        public string Meal
        {
            get
            {
                return this.meal;
            }
            set
            {
                this.meal = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FlightTime
        {
            get
            {
                return this.flightTime;
            }
            set
            {
                this.flightTime = string.IsNullOrEmpty(value) ? string.Empty : value.ToUpper();
            }
        }

        public string GroundTime
        {
            get
            {
                return this.groundTime;
            }
            set
            {
                this.groundTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string TotalTravelTime
        {
            get
            {
                return this.totalTravelTime;
            }
            set
            {
                this.totalTravelTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ActualMileage
        {
            get
            {
                return this.actualMileage;
            }
            set
            {
                this.actualMileage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MileagePlusMileage
        {
            get
            {
                return this.mileagePlusMileage;
            }
            set
            {
                this.mileagePlusMileage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EMP
        {
            get
            {
                return this.emp;
            }
            set
            {
                this.emp = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TotalMileagePlusMileage
        {
            get
            {
                return this.totalMileagePlusMileage;
            }
            set
            {
                this.totalMileagePlusMileage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ClassOfService
        {
            get
            {
                return this.classOfService;
            }
            set
            {
                this.classOfService = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ClassOfServiceDescription
        {
            get
            {
                return this.classOfServiceDescription;
            }
            set
            {
                this.classOfServiceDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBPNRSeat> Seats
        {
            get
            {
                return this.seats;
            }
            set
            {
                this.seats = value;
            }
        }

        public string NumberOfStops
        {
            get
            {
                return this.numberOfStops;
            }
            set
            {
                this.numberOfStops = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBPNRSegment> Stops
        {
            get
            {
                return this.stops;
            }
            set
            {
                this.stops = value;
            }
        }

        public string ScheduledDepartureDateTimeGMT
        {
            get
            {
                return this.scheduledDepartureDateTimeGMT;
            }
            set
            {
                this.scheduledDepartureDateTimeGMT = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ScheduledArrivalDateTimeGMT
        {
            get
            {
                return this.scheduledArrivalDateTimeGMT;
            }
            set
            {
                this.scheduledArrivalDateTimeGMT = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBAirline CodeshareCarrier
        {
            get
            {
                return this.codeshareCarrier;
            }
            set
            {
                this.codeshareCarrier = value;
            }
        }

        public string CodeshareFlightNumber
        {
            get
            {
                return this.codeshareFlightNumber;
            }
            set
            {
                this.codeshareFlightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBSegmentResponse UpgradeVisibility
        {
            get
            {
                return this.upgradeVisibility;
            }
            set
            {
                this.upgradeVisibility = value;
            }
        }

        public bool Upgradeable
        {
            get
            {
                return this.upgradeable;
            }
            set
            {
                this.upgradeable = value;
            }
        }

        public string UpgradeableMessageCode
        {
            get
            {
                return this.upgradeableMessageCode;
            }
            set
            {
                this.upgradeableMessageCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string UpgradeableMessage
        {
            get
            {
                return this.upgradeableMessage;
            }
            set
            {
                this.upgradeableMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ComplimentaryInstantUpgradeMessage
        {
            get
            {
                return this.complimentaryInstantUpgradeMessage;
            }
            set
            {
                this.complimentaryInstantUpgradeMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool Remove
        {
            get
            {
                return this.remove;
            }
            set
            {
                this.remove = value;
            }
        }

        public bool Waitlisted
        {
            get
            {
                return this.waitlisted;
            }
            set
            {
                this.waitlisted = value;
            }
        }

        public int LowestEliteLevel
        {
            get
            {
                return this.lowestEliteLevel;
            }
            set
            {
                this.lowestEliteLevel = value;
            }
        }

        public string UpgradeableRemark
        {
            get
            {
                return this.upgradeableRemark;
            }
            set
            {
                this.upgradeableRemark = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ActionCode
        {
            get
            {
                return this.actionCode;
            }
            set
            {
                this.actionCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public bool UpgradeEligible
        {
            get
            {
                return this.upgradeEligible;
            }
            set
            {
                this.upgradeEligible = value;
            }
        }

        public string WaitedCOSDesc
        {
            get
            {
                return this.waitedCOSDesc;
            }
            set
            {
                this.waitedCOSDesc = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<LmxProduct> LmxProducts
        {
            get
            {
                return this.lmxProducts;
            }
            set
            {
                this.lmxProducts = value;
            }
        }

        public string CabinType
        {
            get
            {
                return this.cabinType;
            }
            set
            {
                this.cabinType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool NonPartnerFlight
        {
            get
            {
                return this.nonPartnerFlight;
            }
            set
            {
                this.nonPartnerFlight = value;
            }
        }

        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        public string FareBasisCode
        {
            get { return fareBasisCode; }
            set { fareBasisCode = value; }
        }
    }
}
