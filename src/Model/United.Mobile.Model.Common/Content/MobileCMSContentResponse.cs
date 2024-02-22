using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Common
{
    public class MobileCMSContentResponse: MOBResponse
    {
        public string SessionId { get; set; } = string.Empty;
        public string CartId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string MileagePlusNumber { get; set; }
        public List<MobileCMSContentMessages> MobileCMSContentMessages { get; set; }
    }

    public class MobileCMSContentMessages
    {
        public string ContentFull { get; set; } = string.Empty;
        public string ContentShort { get; set; } = string.Empty;
        public string HeadLine { get; set; } = string.Empty;
        public string LocationCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }
}
