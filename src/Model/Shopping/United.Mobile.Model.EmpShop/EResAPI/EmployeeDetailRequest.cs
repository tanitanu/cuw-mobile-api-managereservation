using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
 
    public class EmployeeDetailRequest : BaseRequest
    {
        public string DependantId { get; set; }
        public bool FlightWatchOptinStatus { get; set; }
        public string EmailNotificationDays { get; set; }
        public DayOfTravelNotification TravelNotifications { get; set; }
        public DayOfContactInformation ContactInformations { get; set; }
        public bool IsPassriderInfo { get; set; }
        public bool IsPassriderAccess { get; set; }
        public List<PassriderPermission> PassriderPermissions { get; set; }
        public bool DisplayVacation { get; set; }
        public bool AllowPayrollDeduct { get; set; }
        public bool ViewTravelPlan { get; set; }
        public bool ViewEPassDetail { get; set; }
        public int UserProfileId { get; set; }
    }
}

