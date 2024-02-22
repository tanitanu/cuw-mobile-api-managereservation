using System;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class CrediCardAdCCEPromoBanner : CCAdCCEPromoBanner
    {
        private string statementCredit;

        public string StatementCredit
        {
            get { return statementCredit; }
            set { statementCredit = value; }
        }

        //public string statementCredit { get; set; }
        private string placementLandingPageURL;

        public string PlacementLandingPageURL
        {
            get { return placementLandingPageURL; }
            set { placementLandingPageURL = value; }
        }

    }
}
