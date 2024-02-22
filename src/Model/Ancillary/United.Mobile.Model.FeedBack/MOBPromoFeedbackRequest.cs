using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.FeedBack
{
    [Serializable()]
    public class MOBPromoFeedbackRequest : MOBRequest
    {
        public MOBPromoFeedbackEventType EventType { get; set; }

        public string MessageKey { get; set; } = string.Empty;

        public string MileagePlusAccountNumber { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;


    }

    public enum MOBPromoFeedbackEventType
    {
        PRESENTED,
        CLICK
    }

}
