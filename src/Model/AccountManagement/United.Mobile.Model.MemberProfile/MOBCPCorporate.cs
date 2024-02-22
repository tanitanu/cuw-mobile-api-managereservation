using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBCPCorporate
    {

        public int NoOfTravelers { get; set; }

        public string CorporateBookingType { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string DiscountCode { get; set; } = string.Empty;

        public string LeisureDiscountCode { get; set; } = string.Empty;

        public List<ErrorInfo> Errors { get; set; }

        public string FareGroupId { get; set; } = string.Empty;

        public bool IsValid { get; set; }

        public long VendorId { get; set; }

        public string VendorName { get; set; } = string.Empty;

        public MOBCPCorporate()
        {
        }
    }
}
