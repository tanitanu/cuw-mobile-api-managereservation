using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MPSignIn 
    {
        private string objectName = "United.Persist.Definition.Profile.MPSignIn";
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
        public string SessionId { get; set; }
        public string MPNumber { get; set; }
        public string MPHashValue { get; set; }
        public int CustomerId { get; set; }
        public MOBCPProfile Profile { get; set; }
        public string AuthToken { get; set; }
        public DateTime TokenExpirationDateTime { get; set; }
        public Double TokenExpirationSeconds { get; set; }
        public bool IsSignInWithTouchID { get; set; }
    }
}
