using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Service.Presentation.PaymentResponseModel;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ReservationResponse : MOBResponse
    {
     
        public MOBSHOPReservation Reservation { get; set; }
   
        public MOBShoppingCart ShoppingCart { get; set; }

        public List<FormOfPaymentOption> EligibleFormofPayments { get; set; }

        public ReservationResponse()
        {
            EligibleFormofPayments = new List<FormOfPaymentOption>();
        }

    }
}
