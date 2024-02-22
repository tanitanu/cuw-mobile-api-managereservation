using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBSHOPFareWheelItem
    {
        private string key = string.Empty;
        private string tripId = string.Empty;
        private string productId = string.Empty;
        private string displayValue = string.Empty;
        private string value = string.Empty;

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TripId
        {
            get
            {
                return this.tripId;
            }
            set
            {
                this.tripId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ProductId
        {
            get
            {
                return this.productId;
            }
            set
            {
                this.productId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DisplayValue
        {
            get
            {
                return this.displayValue;
            }
            set
            {
                this.displayValue = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
