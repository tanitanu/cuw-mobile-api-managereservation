using System;
using System.Configuration;
using System.Collections.Generic;
using United.Mobile.Model;

namespace United.Mobile.Model.TeaserPage
{
    [Serializable]
    public class MOBSHOPShoppingTeaserPageResponse : MOBResponse
    {
        //private string screenTitle = ConfigurationManager.AppSettings["CompareFareTypesTitle"] ?? string.Empty;
        private string screenTitle = "Compare fare types" ?? string.Empty;
        public string ScreenTitle
        {
            get { return screenTitle; }
            set { screenTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private List<MOBSHOPShoppingTripFareType> columns;
        public List<MOBSHOPShoppingTripFareType> Columns
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
        private string footerText = string.Empty;
        public string FooterText
        {
            get { return footerText; }
            set { footerText = value; }
        }

    }
}
