using System;
using United.Mobile.Model.UpdateMemberProfile;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class ProfileFOPCreditCardResponse : ProfileResponse // ---------DERIVED
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.FOP.ProfileFOPCreditCardResponse";
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

        public MOBUpdateProfileOwnerFOPResponse customerProfileResponse { get; set; }
    }
}
