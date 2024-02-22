using System;
using System.Collections.Generic;
using United.Mobile.Model.Catalog;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CatalogResponse : MOBResponse
    {
        public CatalogResponse()
            : base()
        {
        }

        private List<MOBItem> items;
        private bool succeed = false;
        private List<MOBOptimizelyQMData> experimentEvents;
        public List<MOBItem> Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        public bool Succeed
        {
            get
            {
                return this.succeed;
            }
            set
            {
                this.succeed = value;
            }
        }

        public List<MOBOptimizelyQMData> ExperimentEvents
        {
            get
            {
                return this.experimentEvents;
            }
            set
            {
                this.experimentEvents = value;
            }
        }
    }
}
