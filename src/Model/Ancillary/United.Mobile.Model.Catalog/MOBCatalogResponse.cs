using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Catalog
{
    [Serializable()]
    public class MOBCatalogResponse : MOBResponse
    {

        public List<MOBItem> Items { get; set; }
        public bool Succeed { get; set; }
        public List<MOBOptimizelyQMData> ExperimentEvents { get; set; }

        public MOBCatalogResponse()
        {
            Items = new List<MOBItem>();
            ExperimentEvents = new List<MOBOptimizelyQMData>();
        }

    }
}
