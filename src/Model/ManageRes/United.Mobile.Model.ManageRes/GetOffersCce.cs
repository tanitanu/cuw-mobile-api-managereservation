namespace United.Mobile.Model.ManageRes
{
    public class GetOffersCce
    {
        public GetOffersCce() { }

        #region IPersist Members
        private string objectName = "United.Persist.Definition.Merchandizing.GetOffersCce";

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

        private string offerResponseJson;

        public string OfferResponseJson
        {
            get { return offerResponseJson; }
            set { offerResponseJson = value; }
        }

    }
}
