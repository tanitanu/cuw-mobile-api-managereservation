using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.Payment
{
    public class DataVaultRequest
    {
        public string Amount { get; set; }
        [Required]
        public string SessionId { get; set; }
        public CreditCard CreditCard { get; set; }        
        public string EncryptedCC { get; set; }
    }
}
