using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class PrefRewardProgram
    {
        public long CustomerId { get; set; } 
        public long ProfileId { get; set; } 

        public long PreferenceId { get; set; } 
        public string Key { get; set; } = string.Empty;
       
        public string ProgramMemberId { get; set; } = string.Empty;
       
        public string SourceDescription { get; set; } = string.Empty;
      
        public string SourceCode { get; set; } = string.Empty;
       
        public string VendorCode { get; set; } = string.Empty;
      
        public string VendorDescription { get; set; } = string.Empty;
       
        public int ProgramId { get; set; } 
        public string ProgramCode { get; set; } = string.Empty;
       
        public string ProgramDescription { get; set; } = string.Empty;
       
        public string ProgramType { get; set; } = string.Empty;
       
        public bool IsSelected { get; set; } 

        public bool IsNew { get; set; } 
        public bool IsValidNumber { get; set; }
        private string pin { get; set; }
        private string languageCode { get; set; }
    }
}
