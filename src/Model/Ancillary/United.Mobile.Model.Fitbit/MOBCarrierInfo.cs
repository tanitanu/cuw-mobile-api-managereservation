using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace United.Mobile.Model
{
    [Serializable()]
    public class MOBCarrierInfo
    {
        public string Code { get; set; } = string.Empty;
        
        public string Name { get; set; } = string.Empty;
    }
}
