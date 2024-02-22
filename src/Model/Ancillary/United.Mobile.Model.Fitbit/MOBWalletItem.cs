using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.FeedBack;

namespace United.Mobile.Model.Fitbit
{
    [Serializable()]
    public class MOBWalletItem
    {
        public List<MOBKVP> KVPs { get; set; }
        public MOBWalletItem()
        {
            KVPs = new List<MOBKVP>();
        }
    }
}
