using System;
using System.Configuration;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ShoppingTeaserPageResponse : MOBResponse
    {
        private string screenTitle = ConfigurationManager.AppSettings["CompareFareTypesTitle"] ?? string.Empty;
        public string ScreenTitle
        {
            get { return screenTitle; }
            set { screenTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private List<ShoppingTripFareType> columns;
        public List<ShoppingTripFareType> Columns
        {
            get
            {
                return this.columns;
            }
            set
            {
                this.columns = value;
            }
        }
    }
}

