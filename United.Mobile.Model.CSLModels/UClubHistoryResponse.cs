using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class UClubHistoryResponse : Base
    {
        public string _id { get; set; }
        //
        // Summary:
        //     Gets or sets OnePassNumber.
        public string MpNumber { get; set; }
        public List<UClubMembershipData> UClubMembershipData { get; set; }
    }
}
