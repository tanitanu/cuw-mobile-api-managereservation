using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBBKFormOfPayment
    {
        private string formOfPaymentType = string.Empty;
        private MOBCreditCard creditCard;

        public string FormOfPaymentType
        {
            get
            {
                return this.formOfPaymentType;
            }
            set
            {
                this.formOfPaymentType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public MOBCreditCard CreditCard
        {
            get
            {
                return this.creditCard;
            }
            set
            {
                this.creditCard = value;
            }
        }
    }
}
