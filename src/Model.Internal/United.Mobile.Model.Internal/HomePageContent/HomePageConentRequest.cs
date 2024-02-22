using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.Internal.HomePageContent
{
    public class HomePageConentRequest
    {
        [Required]
        public string EmployeeId { get; set; }        
        public string TransactionId { get; set; }
        [Required]
        public bool IsAgentToolLogOn { get; set; }
        [Required]
        public bool IsPassRiderLoggedIn { get; set; }
        public string SessionId { get; set; }

    }
}
