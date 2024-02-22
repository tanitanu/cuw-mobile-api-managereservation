using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    public class Buddy
    {
        /// <summary>
        /// Gets or sets the buddy ID
        /// </summary>
        public long ID { get; set; } = 0;

        /// <summary>
        /// Gets or sets the OwnerCarrier
        /// </summary>
        public string OwnerCarrier { get; set; } = null;

        /// <summary>
        /// Gets or sets the EmployeeId
        /// </summary>
        public string EmployeeId { get; set; } = null;

        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
        public string FirstName { get; set; } = null;

        /// <summary>
        /// Gets or sets the KnownTraveler
        /// </summary>
        public string KnownTraveler { get; set; } = null;

        /// <summary>
        /// Gets or sets the MiddleName
        /// </summary>
        public string MiddleName { get; set; } = null;

        /// <summary>
        /// Gets or sets the LastName
        /// </summary>
        public string LastName { get; set; } = null;

        /// <summary>
        /// Gets or sets the NameSuffix
        /// </summary>
        public string NameSuffix { get; set; } = null;

        /// <summary>
        /// Gets or sets the Redress
        /// </summary>
        public string Redress { get; set; } = null;

        /// <summary>
        /// Gets or sets the Age
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the BirthDate
        /// </summary>
        public string BirthDate { get; set; } = null;

        /// <summary>
        /// Gets or sets the Gender
        /// </summary>
        public string Gender { get; set; } = null;

        /// <summary>
        /// Gets or sets the Relationship
        /// </summary>
        public RelationshipObject Relationship { get; set; }

        /// <summary>
        /// Gets or sets the DayOfPhone
        /// </summary>
        public string DayOfPhone { get; set; } = null;

        /// <summary>
        /// Gets or sets the DayOfEmail
        /// </summary>
        public string DayOfEmail { get; set; } = null;
    }
}
