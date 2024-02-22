using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ComEmailAddress
    {
        private string address = string.Empty;
        private string defaultIndicator = string.Empty;

        public string Address
        {
            get
            {
                return this.address;
            }
            set
            {
                this.address = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicator;
            }
            set
            {
                this.defaultIndicator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
