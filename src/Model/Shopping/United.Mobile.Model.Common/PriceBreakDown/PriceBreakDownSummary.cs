using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.PriceBreakDown
{
    [Serializable()]
    public class PriceBreakDownSummary
    {
        private List<PriceBreakDown2Items> trip;

        public List<PriceBreakDown2Items> Trip
        {
            get
            {
                return this.trip;
            }
            set
            {
                this.trip = value;
            }
        }

        private List<PriceBreakDown2Items> travelOptions;

        public List<PriceBreakDown2Items> TravelOptions
        {
            get
            {
                return this.travelOptions;
            }
            set
            {
                this.travelOptions = value;
            }
        }

        private List<PriceBreakDown2Items> total;

        public List<PriceBreakDown2Items> Total
        {
            get
            {
                return this.total;
            }
            set
            {
                this.total = value;
            }
        }

        private PriceBreakDown2Items fareLock;

        public PriceBreakDown2Items FareLock
        {
            get
            {
                return this.fareLock;
            }
            set
            {
                this.fareLock = value;
            }
        }
    }
}
