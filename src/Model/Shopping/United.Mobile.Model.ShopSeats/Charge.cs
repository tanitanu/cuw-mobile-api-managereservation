using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.ShopSeats
{
    public class Charge
    {
        public virtual double Amount { get; set; }
        public virtual string Code { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual string Description { get; set; }
        public virtual string Name { get; set; }
        public virtual string Status { get; set; }
        public virtual string Type { get; set; }
    }
}
