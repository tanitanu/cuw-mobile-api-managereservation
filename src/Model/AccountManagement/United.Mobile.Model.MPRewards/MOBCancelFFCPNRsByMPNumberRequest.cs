using United.Mobile.Model.Common;

namespace United.Mobile.Model.MPRewards
{
    public class MOBCancelFFCPNRsByMPNumberRequest : MOBRequest
    {
        public string MileagePlusNumber { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public string HashValue { get; set; } = string.Empty;
    }
}
