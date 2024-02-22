namespace United.Mobile.Model.Common
{
    public class MOBEmployeeProfileTravelTypeRequest
    {
        public string EmployeeId { get; set; }
        public bool IsPassRiderLoggedIn { get; set; }
        public bool IsLogOn { get; set; }
        public string PassRiderLoggedInID { get; set; }
        public string PassRiderLoggedInUser { get; set; }
        public string SessionId { get; set; }
        public string TransactionId { get; set; }
    }
}
