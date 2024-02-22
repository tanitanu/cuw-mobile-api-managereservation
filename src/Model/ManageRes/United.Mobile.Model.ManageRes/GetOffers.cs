using United.Service.Presentation.ProductResponseModel;

namespace United.Mobile.Model.ManageRes
{
    public class GetOffers : ProductOffer
    {
        public GetOffers() { }

        #region IPersist Members
        private string objectName = "United.Persist.Definition.Merchandizing.GetOffers";

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
