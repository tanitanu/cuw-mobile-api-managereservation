using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable]
    public class MOBBKSelectTripResponse : MOBResponse
    {
        private MOBBKSelectTripRequest request;
        private MOBBKAvailability availability;
        private List<string> disclaimer;

        public MOBBKSelectTripRequest Request
        {
            get
            {
                return this.request;
            }
            set
            {
                this.request = value;
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
