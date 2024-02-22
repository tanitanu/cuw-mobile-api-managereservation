using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBUmnr
    {
        public List<MOBItem> Messages { get; set; }
        public MOBUmnr()
        {
            Messages = new List<MOBItem>();
        }
    }
}
