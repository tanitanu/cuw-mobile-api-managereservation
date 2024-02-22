using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.ShopSeats
{
    public class Characteristic
    {
        public Genre Genre { get; set; }        
        public string Code { get; set; }        
        public string Value { get; set; }        
        public string Description { get; set; }        
        public Status Status { get; set; }
        
    }
}
