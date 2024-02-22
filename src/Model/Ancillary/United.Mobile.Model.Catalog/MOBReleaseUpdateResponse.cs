using System;

namespace United.Mobile.Model.Catalog
{
    [Serializable()]
    public class MOBReleaseUpdateResponse : MOBResponse
    {
        public string DisplayMessage { get; set; } = string.Empty;

        public string CTARemindMe { get; set; } = string.Empty;
        public string CTAUpdateNow { get; set; } = string.Empty;
        public string CTADismiss { get; set; } = string.Empty;
        public int RemindMeInterval_Hours { get; set; }
        public string RemindMeInterval_DateTime { get; set; } = string.Empty;
        public int LogIt { get; set; }
    }
}
