namespace United.Mobile.Model.Internal.Payment
{
    public class EmpDataVaultRequest
    {
        public string Amount { get; set; }
        public string BookingSessionId { get; set; }
        public EmpCreditCard CreditCard { get; set; }
        public string EncryptedCC { get; set; }
    }
}
