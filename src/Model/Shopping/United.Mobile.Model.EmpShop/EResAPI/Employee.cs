using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace United.Mobile.Model.Common
{
    public class Employee
    {
        /// <summary>
        /// Gets or sets the Employee Id
        /// </summary>
        public string EmployeeID { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Level1 value.
        /// </summary>
        public string Level1 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Level2 value
        /// </summary>
        public string Level2 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets First Name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Middle Name
        /// </summary>
        public string MiddleName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Last Name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Name Suffix
        /// </summary>
        public string NameSuffix { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Birth Date
        /// </summary>
        public string BirthDate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Gender
        /// </summary>
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Status
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Commute Only
        /// </summary>
        public string CommuteOnly { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Commute Origin
        /// </summary>
        public string CommuteOrigin { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Commute Destination
        /// </summary>
        public string CommuteDestination { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Check Cash SW
        /// </summary>
        public string CheckCashSW { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Check Cash Authentication
        /// </summary>
        public string CheckCashAuth { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets
        /// </summary>
        public string CityCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Mail Code
        /// </summary>
        public string MailCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Employer Id
        /// </summary>
        public string EmployerID { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Employer Description
        /// </summary>
        public string EmployerDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether it is Other Airlines Travel Indicator
        /// </summary>
        public bool OATravelIndicator { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether it is UA Discount Indicator
        /// </summary>
        public bool UADiscountIndicator { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether it is Personal Limited
        /// </summary>
        public bool PersonalLimited { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether it is Annual Travel Card
        /// </summary>
        public bool AnnualTravelCard { get; set; } = false;

        /// <summary>
        /// Gets or sets Travel Region
        /// </summary>
        public string TravelRegion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Comments
        /// </summary>
        public string Comments { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether it is View PBTs
        /// </summary>
        public bool ViewPBTs { get; set; } = false;

        /// <summary>
        /// Gets or sets Carrier Code
        /// </summary>
        public string CarrierCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Global Id
        /// </summary>
        public string GlobalID { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets SSRs
        /// </summary>
        public List<SSRInfo> SSRs { get; set; }

        /// <summary>
        /// Gets or sets Only Family Buddies
        /// </summary>
        public string OnlyFamilyBuddies { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Regional Information
        /// </summary>
        public RegionalInformation RegionalInformation { get; set; }

        /// <summary>
        /// Gets or sets Day Of Contact Information 
        /// </summary>
        public DayOfContactInformation DayOfContactInformation { get; set; }

        /// <summary>
        /// Gets or sets Alt Employee Id
        /// </summary>
        public string AltEmpId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Self Ticket Authentication Indicator
        /// </summary>
        public string SelfTicketAuthInd { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets PS Ticket Authentication Indicator
        /// </summary>
        public string PSTicketAuthInd { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Emergency Ticket Authentication Indicator
        /// </summary>
        public string EmerTicketAuthInd { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Sort Order
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// Gets or sets Day of Travel Notification 
        /// </summary>
        public DayOfTravelNotification DayOfTravelNotification { get; set; }
    }
}
