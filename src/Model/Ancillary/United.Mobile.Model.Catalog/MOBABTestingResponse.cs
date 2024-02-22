using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Catalog
{
    [Serializable]
    public class MOBABTestingResponse : MOBResponse
    {
        public List<MOBABSwitchOption> Items { get; set; }
        public MOBABTestingResponse()
        {
            Items = new List<MOBABSwitchOption>();
        }
    }
}
