namespace United.Mobile.Model.ManageRes
{
    public class CommonDef
    {
        public CommonDef() { }

        #region IPersist Members
        private string objectName = "United.Persist.Definition.Common.CommonDef";

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

        private string sampleJsonResponse = "";

        public string SampleJsonResponse
        {

            get
            {
                return this.sampleJsonResponse;
            }
            set
            {
                this.sampleJsonResponse = value;
            }
        }

    }
}
