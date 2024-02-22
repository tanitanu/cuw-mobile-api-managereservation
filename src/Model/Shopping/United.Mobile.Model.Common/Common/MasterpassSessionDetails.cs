using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class MasterpassSessionDetails
    {
        public MasterpassType MasterpassType { get; set; } 

        public string ContactEmailAddress { get; set; } 
        public string ContactPhoneNumber { get; set; } 
        public string SurName { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;

        public string AccountNumberEncrypted { get; set; } = string.Empty;

        public string AccountNumberHMAC { get; set; } = string.Empty;

        public string AccountNumberLastFourDigits { get; set; } = string.Empty;

        public string AccountNumberMasked { get; set; } = string.Empty;

        public string AccountNumberToken { get; set; } = string.Empty;

        public string PersistentToken { get; set; } = string.Empty;
       
        public double Amount { get; set; } 
        public MOBAddress BillingAddress { get; set; } 
        public string OperationID { get; set; } = string.Empty;
       
        public int WalletCategory { get; set; } 
        public string Code { get; set; } = string.Empty;
       

        public int CreditCardTypeCode { get; set; } 
       
        public int Description { get; set; }
       
        public string ExpirationDate { get; set; } = string.Empty;
       
        public string Name { get; set; } = string.Empty;
       
    }

    public class MasterpassType
    {
       
        public string DefaultIndicator { get; set; } = string.Empty;
      
        public string Description { get; set; } = string.Empty;
       

        public string Key { get; set; } = string.Empty;
       
        public string Val { get; set; } = string.Empty;
       
    }
}