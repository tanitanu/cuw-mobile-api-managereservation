using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class RTIMandateContentToDisplayByMarket
    {
        private string headerMsg;
        private string bodyMsg;
        private RTIMandateContentDetail mandateContentDetail;

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
        public RTIMandateContentDetail MandateContentDetail
        {
            get { return mandateContentDetail; }
            set { mandateContentDetail = value; }
        }
    }

    [Serializable()]
    public class RTIMandateContentDetail
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