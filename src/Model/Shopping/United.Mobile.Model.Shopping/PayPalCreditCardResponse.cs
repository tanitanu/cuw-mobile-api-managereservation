using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class PayPalCreditCardResponse :MOBResponse
    {
        public PayPalCreditCardRequest Request { get; set; } 
        public MOBSHOPReservation Reservation { get; set; } 
    }
}
