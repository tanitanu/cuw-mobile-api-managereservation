using System.Text.RegularExpressions;

namespace United.Mobile.Model.Internal.Common
{
    public  class EResBaseRequest
   {
        public string SearchType { get; set; }
        public string TransactionId { get; set; }
        public string BookingTravelType { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string BookingSessionId { get; set; }
        public string EmployeeID { get; set; }        
        public string SecureToken { get; set; }         
        public bool IsChangeSegment { get; set; } 
        public bool IsAgentToolLogOn { get; set; }        
        public bool IsPassRiderLoggedIn { get; set; }       
        public string BoardDate { get; set; }         
        public bool LoadPassRider { get; set; }
        public string PassRiderLoggedInID { get; set; }
        public string PassRiderLoggedInUser { get; set; }
        public static bool IsValidEmployeeId(string employeeId)
        {

            if (string.IsNullOrWhiteSpace(employeeId))
                return false;

            //All the request from UI will be encrypted. If plain value is sent return false since from fiddler, user can get anyemployee details, so do not accept plain employee id
            if (employeeId.Length == 7 || employeeId.Length == 6 || Regex.IsMatch(employeeId, @"^[A-Za-z]{1}[0-9]{6}\z") || Regex.IsMatch(employeeId, @"^[0-9]*$"))
                return false;

            return true;
        }
    }
}
