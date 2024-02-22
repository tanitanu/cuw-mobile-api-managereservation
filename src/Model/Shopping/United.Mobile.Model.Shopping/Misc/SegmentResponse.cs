using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class SegmentResponse
    {
        public string CarrierCode { get; set; } = string.Empty;
        public string ClassOfService { get; set; } = string.Empty;
        public string DepartureDateTime { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public int FlightNumber { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string PreviousSegmentActionCode { get; set; } = string.Empty;
        public string SegmentActionCode { get; set; } = string.Empty;
        public int SegmentNumber { get; set; }
        public string UpgradeRemark { get; set; } = string.Empty;
        public string DecodedUpgradeMessage { get; set; } = string.Empty;
        public string UpgradeMessage { get; set; } = string.Empty;

        public MessageCode UpgradeMessageCode { get; set; }
        public List<UpgradePropertyKeyValue> UpgradeProperties { get; set; }
        public UpgradeEligibilityStatus UpgradeStatus { get; set; }
        public UpgradeType UpgradeType { get; set; }
        public List<SegmentResponse> WaitlistSegments { get; set; }

        private AdvisoryType remarkAdvisoryType { get; set; }
        public SegmentResponse()
        {
            WaitlistSegments = new List<SegmentResponse>();
        }
    }
}
