using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.Internal.PassBalance
{
    public class PassBalanceRequest
    {
        [Required]
        public bool LoadPassRider { get; set; }
        [Required]
        public string SessionId { get; set; }
        [Required]
        public string TransactionId {get; set;}
        [Required]
        public string EmployeeId { get; set; }

    }
}
