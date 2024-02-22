using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBPriPrice
    {
        private MOBComCharge basePrice;
        private MOBComCharge basePriceEquivalent;
        private double discount;
        private List<MOBComCharge> fees;
        private string pointOfSale = string.Empty;
        private string promotionCode = string.Empty;
        private List<MOBComCharge> surcharges;
        private List<MOBComCharge> taxes;
        private List<MOBComCharge> totals;

        public MOBComCharge BasePrice
        {
            get
            {
                return this.basePrice;
            }
            set
            {
                this.basePrice = value;
            }
        }

        public MOBComCharge BasePriceEquivalent
        {
            get
            {
                return this.basePriceEquivalent;
            }
            set
            {
                this.basePriceEquivalent = value;
            }
        }

        public double Discount
        {
            get
            {
                return this.discount;
            }
            set
            {
                this.discount = value;
            }
        }

        public List<MOBComCharge> Fees
        {
            get
            {
                return this.fees;
            }
            set
            {
                this.fees = value;
            }
        }

        public string PointOfSale
        {
            get
            {
                return this.pointOfSale;
            }
            set
            {
                this.pointOfSale = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PromotionCode
        {
            get
            {
                return this.promotionCode;
            }
            set
            {
                this.promotionCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBComCharge> Surcharges
        {
            get
            {
                return this.surcharges;
            }
            set
            {
                this.surcharges = value;
            }
        }

        public List<MOBComCharge> Taxes
        {
            get
            {
                return this.taxes;
            }
            set
            {
                this.taxes = value;
            }
        }

        public List<MOBComCharge> Totals
        {
            get
            {
                return this.totals;
            }
            set
            {
                this.totals = value;
            }
        }
    }
}
