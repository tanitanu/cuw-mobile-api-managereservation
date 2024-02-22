using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class AwardCalendarItem
    {

        private System.DateTime departs;
        private bool hasEconomySaver;
        private bool hasPremiumSaver;

        public System.DateTime Departs
        {
            get
            {
                return this.departs;
            }
            set
            {
                this.departs = value;
            }
        }

        public bool HasEconomySaver
        {
            get
            {
                return this.hasEconomySaver;
            }
            set
            {
                this.hasEconomySaver = value;
            }
        }

        public bool HasPremiumSaver
        {
            get
            {
                return this.hasPremiumSaver;
            }
            set
            {
                this.hasPremiumSaver = value;
            }
        }

    }
}
