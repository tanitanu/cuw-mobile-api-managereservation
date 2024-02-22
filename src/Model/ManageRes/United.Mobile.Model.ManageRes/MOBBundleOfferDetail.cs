using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBBundleOfferDetail
    {
        public string offerDetailHeader;
        public string offerDetailDescription;
        public string offerDetailWarningMessage;

        public string OfferDetailHeader
        {
            get { return offerDetailHeader; }
            set { offerDetailHeader = value; }
        }

        public string OfferDetailDescription
        {
            get { return offerDetailDescription; }
            set { offerDetailDescription = value; }
        }

        public string OfferDetailWarningMessage
        {
            get { return offerDetailWarningMessage; }
            set { offerDetailWarningMessage = value; }
        }

    }
}
