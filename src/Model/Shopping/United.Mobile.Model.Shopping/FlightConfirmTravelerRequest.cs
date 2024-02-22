using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlightConfirmTravelerRequest : MOBRequest
    {
        public string SessionId { get; set; } = string.Empty;
      
        public MOBSHOPTraveler Traveler { get; set; } 
        
        public string ProfileOption { get; set; } = string.Empty;
       
        public string SecureTravelerOption { get; set; } = string.Empty;
       
        public bool SkipSeats { get; set; } 
        public int PaxIndex { get; set; } 

        public string SequenceNumber { get; set; } = string.Empty;
       
        public FlightConfirmTravelerRequest()
            : base()
        {
        }
    }
}
