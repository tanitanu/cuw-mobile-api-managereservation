using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class AccountProfileInfo
    {
        /// <summary>
        /// Gets or sets account id.
        /// </summary>
        [DataMember]
        public string AccountId { get; set; }

        /// <summary>
        /// Gets or sets legacy account id.
        /// </summary>
        [DataMember]
        public string LegacyAccountId { get; set; }

        /// <summary>
        /// Gets or sets account customer id.
        /// </summary>
        [DataMember]
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets birth date.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gets or sets donor account id.
        /// </summary>
        [DataMember]
        public string DonorAccountId { get; set; }

        /// <summary>
        /// Gets or sets merge survivor account id.
        /// </summary>
        [DataMember]
        public string MergeSurvivorAccountId { get; set; }

        /// <summary>
        /// Gets or sets merge survivor last name.
        /// </summary>
        [DataMember]
        public string MergeSurvivorLastName { get; set; }

        /// <summary>
        /// Gets or sets merge date.
        /// </summary>
        [DataMember]
        public DateTime MergeDate { get; set; }

        /// <summary>
        /// Gets or sets elite level.
        /// </summary>
        [DataMember]
        public string EliteLevel { get; set; }

        /// <summary>
        /// Gets or sets enroll date.
        /// </summary>
        [DataMember]
        public DateTime EnrollDate { get; set; }

        /// <summary>
        /// Gets or sets enroll source.
        /// </summary>
        [DataMember]
        public string EnrollSource { get; set; }

        /// <summary>
        /// Gets or sets current balance.
        /// </summary>
        [DataMember]
        public int CurrentBalance { get; set; }

        /// <summary>
        /// Gets or sets premier qualifying miles balance.
        /// </summary>
        [DataMember]
        public int PremierQualifyingMilesBalance { get; set; }

        /// <summary>
        /// Gets or sets premier qualifying segment balance.
        /// </summary>
        [DataMember]
        public decimal PremierQualifyingSegmentBalance { get; set; }

        /// <summary>
        /// Gets or sets premier qualifying segment floating point component.
        /// </summary>
        [DataMember]
        public string PremierQualifyingSegmentBalanceDecimal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is active.
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is ceo flagged.
        /// </summary>
        [DataMember]
        public bool IsCeo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is closed.
        /// </summary>
        [DataMember]
        public bool IsClosed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is closed permanently.
        /// </summary>
        [DataMember]
        public bool IsClosedPermanently { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is closed temporarily.
        /// </summary>
        [DataMember]
        public bool IsClosedTemporarily { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is united club member.
        /// </summary>
        [DataMember]
        public bool IsUnitedClubMember { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is deceased flagged.
        /// </summary>
        [DataMember]
        public bool IsDeceased { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is infinite elite flagged.
        /// </summary>
        [DataMember]
        public bool IsInfiniteElite { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is a test account.
        /// </summary>
        [DataMember]
        public bool IsTestAccount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is a merge victim.
        /// </summary>
        [DataMember]
        public bool IsMergeVictim { get; set; }

        /// <summary>
        /// Gets or sets gender.
        /// </summary>
        [DataMember]
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets last activity date.
        /// </summary>
        [DataMember]
        public int LastActivityDate { get; set; }

        /// <summary>
        /// Gets or sets last miles expiration activity date.
        /// </summary>
        [DataMember]
        public int MilesExpireLastActivityDate { get; set; }

        /// <summary>
        /// Gets or sets miles expiration date.
        /// </summary>
        [DataMember]
        public int MilesExpireDate { get; set; }

        /// <summary>
        /// Gets or sets last flight date.
        /// </summary>
        [DataMember]
        public int LastFlightDate { get; set; }

        /// <summary>
        /// Gets or sets last statement date.
        /// </summary>
        [DataMember]
        public int LastStatementDate { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        [DataMember]
       public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        [DataMember]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets middle name.
        /// </summary>
        [DataMember]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets suffix.
        /// </summary>
        [DataMember]
        public string Suffix { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets premier qualifying dollars.
        /// </summary>
        [DataMember]
        public int PremierQualifyingDollars { get; set; }

        /// <summary>
        /// Gets or sets chase spending dollars.
        /// </summary>
        [DataMember]
        public int ChaseSpendingDollars { get; set; }

        /// <summary>
        /// Gets or sets the lifetime miles.
        /// </summary>
        [DataMember]
        public int LifetimeMiles { get; set; }

        /// <summary>
        /// Gets or sets the chase spending indicator.
        /// </summary>
        [DataMember]
        public string ChaseSpendingIndicator { get; set; }

        /// <summary>
        /// Gets or sets the presidential plus indicator.
        /// </summary>
        [DataMember]
        public string PresidentialPlusIndicator { get; set; }

        /// <summary>
        /// Gets or sets the chase spending update.
        /// </summary>
        [DataMember]
        public DateTime ChaseSpendingUpdate { get; set; }

        /// <summary>
        /// Gets or sets the million miler status
        /// </summary>
        [DataMember]
        public int MillionMilerLevel { get; set; }

        /// <summary>
        /// Gets or sets the Minimum Segment.
        /// </summary>
        [DataMember]
        public virtual int MinimumSegment { get; set; }
    }
}
