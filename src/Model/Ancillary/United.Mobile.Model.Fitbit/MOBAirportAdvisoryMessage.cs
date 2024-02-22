using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Fitbit
{
    [Serializable()]
    public class MOBAirportAdvisoryMessage
    {
        public string ButtonTitle { get; set; } = string.Empty;
        public string HeaderTitle { get; set; } = string.Empty;

        public List<MOBTypeOption> AdvisoryMessages { get; set; }

        public MOBAirportAdvisoryMessage()
        {
            AdvisoryMessages = new List<MOBTypeOption>();
        }
    }
}
