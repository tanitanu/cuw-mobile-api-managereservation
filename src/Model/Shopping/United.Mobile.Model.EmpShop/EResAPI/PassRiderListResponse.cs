using System.Collections.Generic;


namespace United.Mobile.Model.Common
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

