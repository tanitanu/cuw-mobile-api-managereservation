using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.PassBalance
{
    public class EmpPassBalanceRequest
    {
        [Required]
        public string SessionId { get; set; }
    }
}
