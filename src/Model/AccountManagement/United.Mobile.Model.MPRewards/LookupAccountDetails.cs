using United.Mobile.Model.Common;

namespace United.Mobile.Model.MPRewards
{
    public class LookupAccountDetails
    {
        public string MileagePlusNumber { get; set; }
        public long CustomerId { get; set; }

        public string Balance { get; set; }
        public string BalanceExpireDate { get; set; }
        public string BalanceExpireDisclaimer { get; set; }
        public string NoMileageExpiration { get; set; }
        public string NoMileageExpirationMessage { get; set; }
        public string EnrollDate { get; set; }
        public string LastFlightDate { get; set; }
        public string LastActivityDate { get; set; }
        public string EliteMileage { get; set; }
        public string EliteSegment { get; set; }
        public string LastExpiredMileDate { get; set; }
        public int LastExpiredMile { get; set; }
        public bool HasUAClubMemberShip { get; set; }
        public bool IsMPAccountTSAFlagON { get; set; }
        public string TsaMessage { get; set; }
        public string FourSegmentMinimun { get; set; }
        public string PremierQualifyingDollars { get; set; }
        public string PDQchasewavier { get; set; }
        public string PDQchasewaiverLabel { get; set; }
        public string MillionMilerIndicator { get; set; }
        public byte[] MembershipCardBarCode { get; set; }
        public string MembershipCardBarCodeString { get; set; }
        public bool IsCEO { get; set; }
        public string HashValue { get; set; }
        public string MembershipCardExpirationDate { get; set; }
        public bool ShowChaseBonusTile { get; set; }
        public int LifetimeMiles { get; set; }

        public MOBName Name { get; set; }
    }

}
