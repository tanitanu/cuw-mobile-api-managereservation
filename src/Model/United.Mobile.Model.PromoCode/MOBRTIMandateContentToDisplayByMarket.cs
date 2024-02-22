using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.PromoCode
{
    [Serializable()]
    public class MOBRTIMandateContentToDisplayByMarket
    {
        private string headerMsg;
        private string bodyMsg;
        private MOBRTIMandateContentDetail mandateContentDetail;

        public string HeaderMsg
        {
            get { return headerMsg; }
            set { headerMsg = value; }
        }
        public string BodyMsg
        {
            get { return bodyMsg; }
            set { bodyMsg = value; }
        }
        public MOBRTIMandateContentDetail MandateContentDetail
        {
            get { return mandateContentDetail; }
            set { mandateContentDetail = value; }
        }
    }

    [Serializable()]
    public class MOBRTIMandateContentDetail
    {
        private string navigateToLinkLabel;
        private string navigatePageTitle;
        private string navigatePageBody;

        public string NavigateToLinkLabel
        {
            get { return navigateToLinkLabel; }
            set { navigateToLinkLabel = value; }
        }
        public string NavigatePageTitle
        {
            get { return navigatePageTitle; }
            set { navigatePageTitle = value; }
        }
        public string NavigatePageBody
        {
            get { return navigatePageBody; }
            set { navigatePageBody = value; }
        }
    }
}
