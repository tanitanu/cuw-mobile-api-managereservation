using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{

    [Serializable]
    public class OnTimePerformanceResponse : MOBResponse
    {
        public string SessionId { get; set; } = string.Empty;
        public OnTimePerformanceInformation OnTimePerformance { get; set; }
    }
    [Serializable]
    public class OnTimePerformanceInformation
    {
        public List<MOBTypeOption> OnTimePerformanceItems { get; set; }
       
        public string DotMessagesHtml { get; set; } = string.Empty;
       
        public string OnTimeNotAvailableMessage { get; set; } = string.Empty;
       
        public string TimePeriod { get; set; } = string.Empty;
        public OnTimePerformanceInformation()
        {
            OnTimePerformanceItems = new List<MOBTypeOption>();
        }
    }
}
