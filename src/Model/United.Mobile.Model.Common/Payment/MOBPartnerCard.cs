using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBPartnerCard
    {
        private string cardType = string.Empty;
        private string cardTypeDescription = string.Empty;
        private string key = string.Empty;
        private string mileagePlusnumber = string.Empty;
        private string partnerCode = string.Empty;

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

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string MileagePlusnumber
        {
            get
            {
                return this.mileagePlusnumber;
            }
            set
            {
                this.mileagePlusnumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string PartnerCode
        {
            get
            {
                return this.partnerCode;
            }
            set
            {
                this.partnerCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
