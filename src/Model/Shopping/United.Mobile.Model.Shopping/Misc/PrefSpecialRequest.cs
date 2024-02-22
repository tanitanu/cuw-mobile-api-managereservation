using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class PrefSpecialRequest
    {
        public long AirPreferenceId { get; set; } 
        public long SpecialRequestId { get; set; } 

        public string Key { get; set; } = string.Empty;
      
        public string LanguageCode { get; set; } = string.Empty;
        
        public string SpecialRequestCode { get; set; } = string.Empty;
     
        public string Description { get; set; } = string.Empty;
       
        public long Priority { get; set; } 
        public bool IsNew { get; set; } 

        public bool IsSelected { get; set; } 
    }
}
