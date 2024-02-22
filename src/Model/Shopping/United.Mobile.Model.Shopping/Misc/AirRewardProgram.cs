using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class AirRewardProgram
    {
        public string CustomerId { get; set; } = string.Empty;
     
        public string ProfileId { get; set; } = string.Empty;
       
        public string ProgramCode { get; set; } = string.Empty;
     
        public string ProgramDescription { get; set; } = string.Empty;
      
        public string VendorCode { get; set; } = string.Empty;
       
        public string VendorDescription { get; set; } = string.Empty;
     
        public string ProgramMemberId { get; set; } = string.Empty;
       
        public int EliteLevel { get; set; } 

        public string EliteLevelDescription { get; set; } = string.Empty;
      
    }
}
