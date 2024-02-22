using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.EmployeeProfile
{
    public class Employee
    {
        public string EmployeeID { get; set; } = string.Empty;
        public string Level1 { get; set; } = string.Empty;
        public string Level2 { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string NameSuffix { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string CommuteOnly { get; set; } = string.Empty;
        public string CommuteOrigin { get; set; } = string.Empty;
        public string CommuteDestination { get; set; } = string.Empty;
        public string CheckCashSW { get; set; } = string.Empty;
        public string CheckCashAuth { get; set; } = string.Empty;
        public string CityCode { get; set; } = string.Empty;
        public string MailCode { get; set; } = string.Empty;
        public string EmployerID { get; set; } = string.Empty;
        public string EmployerDescription { get; set; } = string.Empty;
        public bool OATravelIndicator { get; set; } = false;
        public bool UADiscountIndicator { get; set; } = false;
        public bool PersonalLimited { get; set; } = false;
        public bool AnnualTravelCard { get; set; } = false;
        public string TravelRegion { get; set; } = string.Empty;        
        public string Comments { get; set; } = string.Empty;
        public bool ViewPBTs { get; set; } = false;
        public string CarrierCode { get; set; } = string.Empty;
        public string GlobalID { get; set; } = string.Empty;
        public List<SSRInfo> SSRs { get; set; }
        public string OnlyFamilyBuddies { get; set; } = string.Empty;
        public RegionalInformation RegionalInformation { get; set; }
        public DayOfContactInformation DayOfContactInformation { get; set; }
        public string AltEmpId { get; set; } = string.Empty;
        public string SelfTicketAuthInd { get; set; } = string.Empty;
        public string PSTicketAuthInd { get; set; } = string.Empty;
        public string EmerTicketAuthInd { get; set; } = string.Empty;
        public int SortOrder { get; set; } = 0;
        public DayOfTravelNotification DayOfTravelNotification { get; set; }
    }
}
