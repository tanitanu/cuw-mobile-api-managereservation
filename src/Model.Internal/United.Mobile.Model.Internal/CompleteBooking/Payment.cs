namespace United.Mobile.Model.Internal.CompleteBooking
{
    public class Payment
    {      
            public string ExpirationDate { get; set; }
            public string SecurityCode { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public int Description { get; set; }
            public int CreditCardTypeCode { get; set; }
            public int PinEntryCapability { get; set; }
            public int InputCapability { get; set; }
            public int TerminalAttended { get; set; }
            public int TerminalCardholderActivated { get; set; }
            public string SecurityCodeToken { get; set; }
            public float Amount { get; set; }
            public Payor Payor { get; set; }
            public string AccountNumber { get; set; }
            public Currency Currency { get; set; }
            public string AccountNumberHMAC { get; set; }
            public string AccountNumberEncrypted { get; set; }
            public string AccountNumberMasked { get; set; }
            public string OperationID { get; set; }
            public string AccountNumberToken { get; set; }
            public string AccountNumberLastFourDigits { get; set; }
            public int WalletCategory { get; set; }
            public string PersistentToken { get; set; }
        

       

    }
    public class Payor
    {
        public string GivenName { get; set; }
        public Contact Contact { get; set; }
    }

    public class Contact
    {
        public string PersonKey { get; set; }
        public object[] Addresses { get; set; }
        public Email[] Emails { get; set; }
        public object[] PhoneNumbers { get; set; }
        public Status Status { get; set; }
        public object[] Devices { get; set; }
    }

    public class Status
    {
    }

    public class Email
    {
        public string Address { get; set; }
    }

    public class Currency
    {
        public string Code { get; set; }
    }
}