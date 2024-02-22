using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class UpdateProfileOwnerFOPResponse : UpdateFOPTravelerResponse
    {
        private MOBSHOPReservation reservation;

        public MOBSHOPReservation Reservation
        {
            get
            {
                return this.reservation;
            }
            set
            {
                this.reservation = value;
            }
        }
    }
}
