using System;
using System.Collections.Generic;
using United.Definition;
using United.Mobile.Model.Common.SSR;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class PNRPassenger
    {
        public MOBLMXTraveler EarnedMiles { get; set; } 
        public MOBContact Contact { get; set; } 
        public string BirthDate { get; set; } = string.Empty;
        public string TravelerTypeCode { get; set; } = string.Empty;
        public string PricingPaxType { get; set; } = string.Empty;
        public string SSRDisplaySequence { get; set; } = string.Empty;
        public List<TravelSpecialNeed> SelectedSpecialNeeds { get; set; } 
        public string SharesGivenName { get; set; } = string.Empty;
        public string PNRCustomerID { get; set; } = string.Empty;
        public string KnownTravelerNumber { get; set; } = string.Empty;
        public string RedressNumber { get; set; } = string.Empty;
        public string KTNDisplaySequence { get; set; } = string.Empty;
        public string REDRESSDisplaySequence { get; set; } = string.Empty;
        public MOBName PassengerName { get; set; } 
        public string SHARESPosition { get; set; } = string.Empty;
        public CPMileagePlus MileagePlus { get; set; } 
        public List<RewardProgram> OaRewardPrograms { get; set; } 
        public bool IsMPMember { get; set; } 
        public MOBSHOPLoyaltyProgramProfile LoyaltyProgramProfile { get; set; } 
        public PNREmployeeProfile EmployeeProfile { get; set; } 
    }
}
