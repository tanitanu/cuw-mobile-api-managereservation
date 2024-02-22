using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ComCharge
    {
        private double amount;
        private string code = string.Empty;
        private ComCurrency currency;
        private string description = string.Empty;
        private string name = string.Empty;

        public double Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                this.amount = value;
            }
        }

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public ComCurrency Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
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

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
