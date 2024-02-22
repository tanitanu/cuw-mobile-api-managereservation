using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBEmpLmxProduct
    {
        private string bookingCode = string.Empty;
        private string description = string.Empty;
        private List<MOBEmpLmxLoyaltyTier> lmxLoyaltyTiers;
        private string productType = string.Empty;

        public string BookingCode
        {
            get
            {
                return this.bookingCode;
            }
            set
            {
                this.bookingCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBEmpLmxLoyaltyTier> LmxLoyaltyTiers
        {
            get
            {
                return this.lmxLoyaltyTiers;
            }
            set
            {
                this.lmxLoyaltyTiers = value;
            }
        }

        public string ProductType
        {
            get
            {
                return this.productType;
            }
            set
            {
                this.productType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
