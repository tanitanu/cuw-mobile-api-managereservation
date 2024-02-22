using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
   public class TravelDocResponse : BaseResponse
    {
        public List<SSRInfo> SSRInfos { get; set; }
        public bool IsSuccess { get; set; }
    }
}

