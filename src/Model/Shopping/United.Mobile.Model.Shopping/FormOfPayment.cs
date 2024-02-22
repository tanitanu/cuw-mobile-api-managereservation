using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FormOfPayment
    {
        public MOBApplePay ApplePayInfo { get; set; } 
        public string FormOfPaymentType { get; set; } = string.Empty;
      
        public MOBCreditCard CreditCard { get; set; }

        public string VISACheckOutCallID { get; set; } = string.Empty;
       
        public MOBFormofPayment formOfPayment { get; set; } 
        
        public MOBPayPal PayPal { get; set; } 
        public MOBMasterpass Masterpass { get; set; } 
        public FormOfPayment()
        {
            this.formOfPayment = MOBFormofPayment.CreditCard;
        }
    }
}
