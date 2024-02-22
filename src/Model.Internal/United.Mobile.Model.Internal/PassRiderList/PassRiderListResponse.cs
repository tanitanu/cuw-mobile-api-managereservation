using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.PassRiderList
{
    public class PassRiderListResponse : EResBaseResponse
    {        
        public List<Passenger> PassengerList { get; set; } 
        public bool IsSJ1UEligible { get; set; }
        public string SJ1UMessage { get; set; }
        public List<Passenger> JumpSeatPassengerList { get; set; }
        public EResAlert JumpSeatAlert { get; set; }
        public PassengerListRequest PassengerListRequest { get; set; }
    }
}

