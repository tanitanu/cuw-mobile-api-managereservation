using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class DOTBaggageTravelerInfo
    {
        private string id = string.Empty;
        private string givenName = string.Empty;
        private string surname = string.Empty;
        private DateTime ticketingDate;
        private string ticketingDateString = string.Empty;
        private DOTBaggageLoyalty loyalty;

        public string Id
        {
            get { return this.id; }
            set { this.id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string GivenName
        {
            get { return this.givenName; }
            set { this.givenName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Surname
        {
            get { return this.surname; }
            set { this.surname = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public DateTime TicketingDate
        {
            get
            {
                return this.ticketingDate;
            }
            set
            {
                this.ticketingDate = value;
            }
        }

        public string TicketingDateString
        {
            get { return this.ticketingDateString; }
            set { this.ticketingDateString = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public DOTBaggageLoyalty Loyalty
        {
            get { return this.loyalty; }
            set { this.loyalty = value; }
        }
    }
}
