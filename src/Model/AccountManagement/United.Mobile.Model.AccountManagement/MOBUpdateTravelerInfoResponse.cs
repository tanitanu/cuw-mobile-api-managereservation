using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Definition;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBUpdateTravelerInfoResponse : MOBResponse
    {
        private string sessionId;
        private string token = string.Empty;
        private List<MOBPNRPassenger> travelersInfo;
        private string mealAccommodationAdvisory = string.Empty;
        private string mealAccommodationAdvisoryHeader = string.Empty;
        public string MealAccommodationAdvisory { get { return this.mealAccommodationAdvisory; } set { this.mealAccommodationAdvisory = value; } }
        public string MealAccommodationAdvisoryHeader { get { return this.mealAccommodationAdvisoryHeader; } set { this.mealAccommodationAdvisoryHeader = value; } }
        public string SessionId { get { return this.sessionId; } set { this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string Token { get { return this.token; } set { this.token = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<MOBPNRPassenger> TravelersInfo { get { return this.travelersInfo; } set { this.travelersInfo = value; } }
    }
}
