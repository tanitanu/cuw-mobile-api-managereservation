using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBEmpCreditCard 
    {

        private string cardType = string.Empty;
        private string cardTypeDescription = string.Empty;
        private string expireMonth = string.Empty;
        private string expireYear = string.Empty;
        private string encryptedCardNumber = string.Empty;
        private string cIDCVV2 = string.Empty;
        private string cCFirstName = string.Empty;
        private string cCLastName = string.Empty;
        private string displayCardNumber = string.Empty;

        public string CardType
        {
            get
            {
                return this.cardType;
            }
            set
            {
                this.cardType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DisplayCardNumber
        {
            get { return this.displayCardNumber; }
            set { this.displayCardNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string CardTypeDescription
        {
            get
            {
                return this.cardTypeDescription;
            }
            set
            {
                this.cardTypeDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ExpireMonth
        {
            get
            {
                return this.expireMonth;
            }
            set
            {
                this.expireMonth = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ExpireYear
        {
            get
            {
                return this.expireYear;
            }
            set
            {
                this.expireYear = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EncryptedCardNumber
        {
            get { return this.encryptedCardNumber; }
            set { this.encryptedCardNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string CIDCVV2
        {
            get { return this.cIDCVV2; }
            set { this.cIDCVV2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string CCFirstName
        {
            get { return this.cCFirstName; }
            set { this.cCFirstName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string CCLastName
        {
            get { return this.cCLastName; }
            set { this.cCLastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
