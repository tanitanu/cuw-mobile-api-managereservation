using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.AlertCheckFSR
{
    public class AlertResponse : EResBaseResponse
    {       
        public List<EResAlert> BaseAlerts { get; set; }
    }
}
