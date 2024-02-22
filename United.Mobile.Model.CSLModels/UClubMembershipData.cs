using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class UClubMembershipData
    {
        public string MemberTypeCode { get; set; }
        //
        // Summary:
        //     Gets or sets MemberType Description.


        public string MemberTypeDescription { get; set; }
        //
        // Summary:
        //     Gets or sets Status.


        public string ClubStatusCode { get; set; }
        //
        // Summary:
        //     Gets or sets Status.


        public string ClubStatusDescription { get; set; }
        //
        // Summary:
        //     Gets or sets StartDate.

        public DateTime EffectiveDate { get; set; }
        //
        // Summary:
        //     Gets or sets EndDate.

        public DateTime DiscontinueDate { get; set; }
        //
        // Summary:
        //     Gets or sets CompanionOnePassNumber.


        public string CompanionMpNumber { get; set; }
        //
        // Summary:
        //     Gets or sets PrimaryOrCompanion.


        public string PrimaryOrCompanion { get; set; }
        //
        // Summary:
        //     Gets or sets LastUpdateDate.

        public DateTime LastUpdateDate { get; set; }
        //
        // Summary:
        //     Gets or sets LastUpdateBy.


        public string LastUpdateBy { get; set; }
        //
        // Summary:
        //     Gets or sets SequenceNumber.

        public int SequenceNumber { get; set; }
    }
}
