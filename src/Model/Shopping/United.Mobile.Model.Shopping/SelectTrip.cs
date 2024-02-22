using System;
using System.Xml.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    [XmlRoot("SelectTrip")]
    //[XmlRoot("MOBSHOPSelectTripResponse")]
    public class SelectTrip //: IPersist
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Shopping.SelectTrip";
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

        public string SessionId { get; set; }

        public string CartId { get; set; }
        public SerializableDictionary<string, SelectTripRequest> Requests { get; set; }
        public SerializableDictionary<string, SelectTripResponse> Responses { get; set; }
        public string LastSelectTripKey { get; set; }
    }
}
