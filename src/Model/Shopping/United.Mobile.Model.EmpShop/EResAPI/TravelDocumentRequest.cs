using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
   public class TravelDocumentRequest : BaseRequest
    {
        public List<SSRInfo> SSRInfos { get; set; }
        public bool IsPassriderCabin { get; set; }
        public string TravelDocumentId { get; set; }
    }
}

