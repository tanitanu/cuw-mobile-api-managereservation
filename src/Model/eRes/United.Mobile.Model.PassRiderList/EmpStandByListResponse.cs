using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.PassRiderList
{
    public class EmpStandByListResponse
    {
        public List<EmpStandByListPassenger> StandByListing {get; set;}
        public List<EmpStandByListPassenger> JumpSeatListing { get; set; }
    }
}
