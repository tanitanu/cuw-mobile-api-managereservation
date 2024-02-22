using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.MPSignIn
{
    [Serializable()]
    public class MOBFutureFlightCredit
    {
        private List<MOBItem> messages;
        public List<MOBItem> Messages { get { return this.messages; } set { this.messages = value; } }
    }
}
