using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class UnregisterFareLockResponse : MOBResponse
    {
        public UnregisterFareLockRequest Request { get; set; }

        public MOBSHOPAvailability Availability { get; set; }

    }
}

