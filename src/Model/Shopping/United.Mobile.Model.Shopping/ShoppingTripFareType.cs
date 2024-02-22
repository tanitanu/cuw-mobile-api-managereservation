using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ShoppingTripFareType
    {
        private string dataSourceLabel = string.Empty;
        public string DataSourceLabel
        {
            get { return dataSourceLabel; }
            set { dataSourceLabel = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string description = string.Empty;
        public string Description
        {
            get { return description; }
            set { description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private List<TeaserText> teaserTexts;
        public List<TeaserText> TeaserTexts
        {
            get
            {
                return this.teaserTexts;
            }
            set
            {
                this.teaserTexts = value;
            }
        }
    }
}
