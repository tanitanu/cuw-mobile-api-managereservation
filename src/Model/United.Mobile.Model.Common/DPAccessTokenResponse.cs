using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class DPAccessTokenResponse
    {
        private string accessToken = string.Empty;
        private string consented_on;
        private int expires_in;
        private string grant_type;
        private string id_token;
        private string scope;
        private string token_type;
        private string mileagePlusNumber;
        private int customerId;
        private string sub;
        private string jti;
        private int errorCode;
        private string errorDescription;
        private string failedAttempts;
        private bool isDPThrownErrors;


        public bool IsDPThrownErrors
        {
            get { return this.isDPThrownErrors; }
            set { this.isDPThrownErrors = value; }
        }

        public string FailedAttempts
        {
            get { return this.failedAttempts; }
            set { this.failedAttempts = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ErrorDescription
        {
            get { return this.errorDescription; }
            set { this.errorDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public int ErrorCode
        {
            get { return this.errorCode; }
            set { this.errorCode = value; }
        }
        public string AccessToken
        {
            get { return this.accessToken; }
            set { this.accessToken = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Consented_on
        {
            get { return this.consented_on; }
            set { this.consented_on = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public int Expires_in
        {
            get { return this.expires_in; }
            set { this.expires_in = value; }
        }
        public string Grant_type
        {
            get { return this.grant_type; }
            set { this.grant_type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Id_token
        {
            get { return this.id_token; }
            set { this.id_token = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Scope
        {
            get
            {
                return this.scope;
            }
            set
            {
                this.scope = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Token_type
        {
            get
            {
                return this.token_type;
            }
            set
            {
                this.token_type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }


        public string Jti
        {
            get { return jti; }
            set { jti = value; }
        }


        public string Sub
        {
            get { return sub; }
            set { sub = value; }
        }



        public int CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }



        public string MileagePlusNumber
        {
            get { return mileagePlusNumber; }
            set { mileagePlusNumber = value; }
        }




    }
}
