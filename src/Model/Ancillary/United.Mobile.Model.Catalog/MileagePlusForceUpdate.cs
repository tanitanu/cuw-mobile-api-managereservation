using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Catalog
{
    public class MileagePlusForceUpdate
    {
        public string DisplayMessage { get; set; } = string.Empty;
        public string CTARemindMe { get; set; } = string.Empty;
        public string CTAUpdateNow { get; set; } = string.Empty;
        public string CTADismiss { get; set; } = string.Empty;
        public int RemindMeInterval_Hours { get; set; }
        public DateTime RemindMeInterval_DateTime { get; set; } 
        public int LogIt { get; set; } 
    }
}
