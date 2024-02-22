using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.PriceBreakDown
{
    [Serializable()]
    public class PriceBreakDownDetails
    {
        private PriceBreakDown2Items trip;
        private List<PriceBreakDown2TextItems> taxAndFees;
        private PriceBreakDownAddServices additionalServices;
        private List<PriceBreakDown2Items> total;
        private List<PriceBreakDown2Items> fareLock;

        public PriceBreakDown2Items Trip
        {
            get
            {
                return trip;
            }
            set
            {
                trip = value;
            }
        }

        public List<PriceBreakDown2TextItems> TaxAndFees
        {
            get
            {
                return taxAndFees;
            }
            set
            {
                taxAndFees = value;
            }
        }
    
        public PriceBreakDownAddServices AdditionalServices
        {
            get
            {
                return additionalServices;
            }
            set
            {
                additionalServices = value;
            }
        }

        public List<PriceBreakDown2Items> Total
        {
            get 
            {
                return total;
            }
            set
            {
                total = value;
            }
        }

        public List<PriceBreakDown2Items> FareLock
        {
            get
            {
                return fareLock;
            }
            set
            {
                fareLock = value;
            }
        }
    
    }
}
