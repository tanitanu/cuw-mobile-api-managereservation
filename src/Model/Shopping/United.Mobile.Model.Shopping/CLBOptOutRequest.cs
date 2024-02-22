using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CLBOptOutRequest : MOBRequest
    {
        public string SessionId { get; set; } = string.Empty;
       
    }
}
