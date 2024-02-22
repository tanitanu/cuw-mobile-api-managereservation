using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBFutureFlightCredit
    {
        public List<MOBItem> Messages { get; set; }

        public MOBFutureFlightCredit()
        {
            Messages = new List<MOBItem>();
        }
    }
}
