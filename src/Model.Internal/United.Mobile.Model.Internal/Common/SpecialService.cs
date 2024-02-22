using System.Collections.Generic;

namespace United.Mobile.Model.Internal.Common
{
    public class SpecialService
    {        
        public long Id { get; set; }
        public string FullServiceName { get; set; }
        public string ServiceCode { get; set; } 
        public List<SpecialService> AdditionalOption { get; set; } 
        public string AlternateServiceName { get; set; }
    }
}
