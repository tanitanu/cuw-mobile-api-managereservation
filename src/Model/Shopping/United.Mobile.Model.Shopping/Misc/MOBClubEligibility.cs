using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ClubEligibility
    {
        private string eligibility = string.Empty;
        private string guests = string.Empty;

        public string Eligibility
        {
            get
            {
                return this.eligibility;
            }
            set
            {
                this.eligibility = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Guests
        {
            get
            {
                return this.guests;
            }
            set
            {
                this.guests = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
