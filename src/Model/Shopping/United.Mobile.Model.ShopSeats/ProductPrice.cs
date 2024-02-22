using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.ShopSeats
{
    public class ProductPrice
    {
        public virtual Charge BasePrice { get; set; }
        public virtual Charge BasePriceEquivalent { get; set; }
        public virtual Collection<Charge> Fees { get; set; }
        public virtual string PromotionCode { get; set; }
        public virtual Collection<Charge> Taxes { get; set; }
        public virtual Collection<Charge> Totals { get; set; }
        public Genre Type { get; set; }
    }
}
