using System;
using United.Mobile.Model.Internal.Exception;

namespace United.Mobile.Model
{
    [Serializable]
    public class MOBResponse
    {
        public string TransactionId { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = "en-US";
        public string MachineName { get; set; } = System.Environment.MachineName;
        public long CallDuration { get; set; }
        public MOBException Exception { get; set; }
    }
}