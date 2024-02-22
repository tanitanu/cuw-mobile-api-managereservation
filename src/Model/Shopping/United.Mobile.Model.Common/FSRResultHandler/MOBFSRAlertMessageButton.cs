using System;


namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBFSRAlertMessageButton
    {

        private string buttonLabel;
        private MOBSHOPShopRequest updatedShopRequest;
        private string redirectUrl;

        /// <summary>
        /// Title of the button
        /// </summary>
        public string ButtonLabel
        {
            get { return buttonLabel; }
            set { buttonLabel = value; }
        }

        /// <summary>
        /// Updated shop request that this button needs to use.
        /// </summary>
        public MOBSHOPShopRequest UpdatedShopRequest
        {
            get { return updatedShopRequest; }
            set { updatedShopRequest = value; }
        }

        public string RedirectUrl
        {
            get { return redirectUrl; }
            set { redirectUrl = value; }
        }

    }
}