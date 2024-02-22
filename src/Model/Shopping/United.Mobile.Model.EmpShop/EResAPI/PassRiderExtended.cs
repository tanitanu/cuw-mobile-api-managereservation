using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    public class PassRiderExtended
    {
        /// <summary>
        /// Gets or sets UserName
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets FirstName
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets LastName
        /// </summary>
        public string LastName { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets MiddleName
        /// </summary>
        public string MiddleName { get; set; } = string.Empty;

        /// </summary>
        public string NameSuffix { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Status Text
        /// </summary>
        public string StatusText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets status
        /// </summary>
        public bool Status { get; set; } = false;

        /// <summary>
        /// Gets or sets Vacation Permitted
        /// </summary>
        public bool VacationPermitted { get; set; } = false;

        /// <summary>
        /// Gets or sets purchase ticket on payroll
        /// </summary>
        public bool PayrollDeductPermitted { get; set; } = false;

        /// <summary>
        /// Gets or sets view the travelplan detail
        /// </summary>
        public bool ViewTravelPlan { get; set; } = false;

        /// <summary>
        /// Gets or sets EPass Detail
        /// </summary>
        public bool ViewEPassDetail { get; set; } = false;

        /// <summary>
        /// Gets or sets Day Of Contact Information
        /// </summary>
        public DayOfContactInformation DayOfContactInformation = null;

        /// <summary>
        /// Gets or Sets Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Sets Fax
        /// </summary>
        public string Fax { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Sets Phone
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Sets DependantId
        /// </summary>
        public string DependantId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Sets EmployeeId
        /// </summary>
        public string EmployeeId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user profile id
        /// </summary>
        public int UserProfileId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the relationship
        /// </summary>
        public RelationshipObject Relationship { get; set; }

        /// <summary>
        /// Gets or sets indicating pass rider is primary friend
        /// </summary>
        public bool IsPrimaryFriend { get; set; }
    }
}
