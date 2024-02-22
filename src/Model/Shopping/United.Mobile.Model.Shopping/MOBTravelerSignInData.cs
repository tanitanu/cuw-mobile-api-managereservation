using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBTravelerSignInData
    {
        private string signedInEliteStatusCode;
        private string signedInRPC;

        public string SignedInEliteStatusCode
        {
            get
            {
                return this.signedInEliteStatusCode;
            }
            set
            {
                this.signedInEliteStatusCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string SignedInRPC
        {
            get
            {
                return this.signedInRPC;
            }
            set
            {
                this.signedInRPC = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
