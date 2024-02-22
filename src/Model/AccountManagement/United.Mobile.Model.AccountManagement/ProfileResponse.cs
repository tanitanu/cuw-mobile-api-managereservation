using System;
using United.Mobile.Model.UpdateMemberProfile;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class ProfileResponse : IPersist
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Shopping.ProfileResponse";
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
        public MOBCustomerProfileRequest Request { get; set; }
        public MOBCustomerProfileResponse Response { get; set; }

        public MOBUpdateFOPTravelerResponse UpdateProfileResponse { get; set; }

    }
}
