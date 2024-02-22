using United.Definition;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.BagCalculator
{
    public class SHOPCheckedBagInfoResponse: MOBResponse
    {
        public SHOPCheckedBagInfoRequest Request { get; set; }
        public CheckedBagChargeInfo CheckedBagChargeInfo { get; set; }
        public string SessionId { get; set; }
        public string CartId { get; set; }
        public MOBSHOPResponseStatusItem ResponseStatusItem { get; set; }
    }
}
