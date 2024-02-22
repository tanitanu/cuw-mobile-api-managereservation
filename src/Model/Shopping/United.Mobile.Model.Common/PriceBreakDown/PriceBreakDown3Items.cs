using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.PriceBreakDown
{
    [Serializable()]
    public class PriceBreakDown3Items : PriceBreakDown2Items
    {
        private string price2;
        public string Price2
        {
            get
            {
                return this.price2;
            }
            set
            {
                this.price2 = value;
            }
        }
    }
}
