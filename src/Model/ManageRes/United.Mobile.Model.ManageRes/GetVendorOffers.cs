
using United.Service.Presentation.ProductResponseModel;

namespace United.Mobile.Model.ManageRes
{
    public class GetVendorOffers : ProductOffer
    {
        public GetVendorOffers() { }

        #region IPersist Members
        private string objectName = "United.Persist.Definition.Merchandizing.GetVendorOffers";

        public string ObjectName
        {

            get
            {
                return this.objectName;
            }
            set
            {
                this.objectName = value;
            }
        }

        #endregion
    }
}
