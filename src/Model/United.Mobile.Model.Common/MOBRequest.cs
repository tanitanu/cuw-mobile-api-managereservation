using System;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBRequest
    {
        public string AccessCode { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public MOBApplication Application { get; set; }
        private string deviceId { get; set; } = string.Empty;

        public string DeviceId
        {
            get
            {
                return this.deviceId;
            }
            set
            {
                this.deviceId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}