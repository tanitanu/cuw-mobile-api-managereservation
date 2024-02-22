using System;
using United.Mobile.Model.Shopping.FormofPayment;

namespace United.Mobile.Model.Shopping
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

        public UpdateProfileOwnerFOPResponse customerProfileResponse { get; set; }
    }
}
