using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.PriceBreakDown
{
    [Serializable()]
    public class PriceBreakDown4Items : PriceBreakDown3Items
    {
        private string price3;
        public string Price3
        {
            get
            {
                return this.price3;
            }
            set
            {
                this.price3 = value;
            }
        }
    }
}
