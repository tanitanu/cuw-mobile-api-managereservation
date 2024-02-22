using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.PriceBreakDown
{
    [Serializable()]
    public class PriceBreakDownAddServices
    {

        private List<PriceBreakDown4Items> seats;
        private List<PriceBreakDown3Items> premiumAccess;
        private List<PriceBreakDown2Items> oneTimePass;

        public List<PriceBreakDown4Items> Seats
        {
            get
            {
                return this.seats;
            }
            set
            {
                this.seats = value;
            }
        }

        public List<PriceBreakDown3Items> PremiumAccess
        {
            get
            {
                return this.premiumAccess;
            }
            set
            {
                this.premiumAccess = value;
            }
        }

        public List<PriceBreakDown2Items> OneTimePass
        {
            get
            {
                return this.oneTimePass;
            }
            set
            {
                this.oneTimePass = value;
            }
        }


    }
}
