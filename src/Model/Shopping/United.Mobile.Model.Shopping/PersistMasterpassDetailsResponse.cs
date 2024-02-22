using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class PersistMasterpassDetailsResponse :MOBResponse
    {
        public PersistMasterpassDetailsRequest Request { get; set; }
        
        public MOBSHOPReservation Reservation { get; set; } 
    }
}
