using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CompleteSeatsRequest : MOBRequest
    {

        public string SessionId { get; set; } = string.Empty;
        
        public string SponsorMPAccountId { get; set; } = string.Empty;
      
        public string SponsorEliteLevel { get; set; } = string.Empty;
       
        public string SeatAssignment { get; set; } = string.Empty;
        
        public string Origin { get; set; } = string.Empty;
       
        public string Destination { get; set; } = string.Empty;
      
        public string PaxIndex { get; set; } = string.Empty;
     

    }
}
