using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.Internal.EmployeeProfile
{
    public class EmployeeProfileRequest
    {
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public bool IsPassRiderLoggedIn { get; set; }
        [Required]
        public bool IsLogOn { get; set; }
        [Required]
        public string PassRiderLoggedInID { get; set; }
        [Required]
        public string PassRiderLoggedInUser { get; set; }
        [Required]
        public string SessionId { get; set; }
        [Required]
        public string TransactionId { get; set; }
    }
}
    