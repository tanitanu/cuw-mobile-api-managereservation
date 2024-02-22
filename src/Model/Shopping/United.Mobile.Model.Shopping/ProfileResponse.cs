using System;
using United.Mobile.Model.Shopping.FormofPayment;

namespace United.Mobile.Model.Shopping

{
    [Serializable()]
    public class ProfileResponse 
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
        public CustomerProfileRequest Request { get; set; }
        public CustomerProfileResponse Response { get; set; }
        
        public UpdateFOPTravelerResponse UpdateProfileResponse { get; set; }

    }
}
