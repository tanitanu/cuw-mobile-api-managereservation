using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKShopResponse : MOBResponse
    {
        private MOBBKShopRequest shopRequest;
        private MOBBKAvailability availability;
        private List<string> disclaimer;

        public MOBBKShopRequest ShopRequest
        {
            get
            {
                return this.shopRequest;
            }
            set
            {
                this.shopRequest = value;
            }
        }

        public MOBBKAvailability Availability
        {
            get
            {
                return this.availability;
            }
            set
            {
                this.availability = value;
            }
        }

        public List<string> Disclaimer
        {
            get
            {
                return this.disclaimer;
            }
            set
            {
                this.disclaimer = value;
            }
        }
    }
}
