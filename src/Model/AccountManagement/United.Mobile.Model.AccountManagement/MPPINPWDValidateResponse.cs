using System;
using System.Xml.Serialization;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    [XmlRoot("MPPINPWDValidateResponse")]
    public class MPPINPWDValidateResponse
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Profile.MPPINPWDValidateResponse";
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
        //public MOBMPPINPWDValidateResponse Response { get; set; }
        public MOBCPProfile Profile { get; set; }
        public MOBMPSecurityUpdateResponse Response { get; set; }
        public MOBEmpTravelTypeResponse EmpTravelTypeResponse { get; set; }

    }
}
