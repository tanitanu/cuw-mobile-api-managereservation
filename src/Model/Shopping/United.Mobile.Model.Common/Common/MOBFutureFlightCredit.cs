using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
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
