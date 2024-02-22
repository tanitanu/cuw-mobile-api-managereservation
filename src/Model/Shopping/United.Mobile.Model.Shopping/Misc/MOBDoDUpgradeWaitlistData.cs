using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    /*This class is not to be exposed to clients (iOS/Android), 
     * we use this to populate data and serialize to json and send in response
     * client will pass the same json in Request to CheckInWF Call*/
    public class DoDUpgradeWaitlistData
    {
        public int FlightNumber { get; set; }
        public int BusinessCheckedInWLCount { get; set; }
        public int PremiumPlusCheckedInWLCount { get; set; }
        public int BusinessNotCheckedInWLCount { get; set; }
        public int PremiumPlusNotCheckedInWLCount { get; set; }
        public string Origin { get; set; }
        public string PaxFirstName { get; set; }
        public string PaxLastName { get; set; }
        public string DepartureDate { get; set; }
    }
}
