using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Internal.Exception;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable]
    public class MOBResponse
    {
        private string transactionId = string.Empty;
        private string languageCode = "en-US";
        private string machineName = System.Environment.MachineName.Substring(System.Environment.MachineName.Length - 2);
        private long callDuration;
        private MOBException exception;

        public string TransactionId
        {
            get
            {
                return this.transactionId;
            }
            set
            {
                this.transactionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string LanguageCode
        {
            get
            {
                return this.languageCode;
            }
            set
            {
                this.languageCode = string.IsNullOrEmpty(value) ? "en-US" : value.Trim();
            }
        }

        public string MachineName
        {
            get
            {
                return this.machineName;
            }
            set
            {
                this.machineName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public long CallDuration
        {
            get
            {
                return this.callDuration;
            }
            set
            {
                this.callDuration = value;
            }
        }

        public MOBException Exception
        {
            get
            {
                return this.exception;
            }
            set
            {
                this.exception = value;
            }
        }
    }

    [Serializable]
    public class MOBWResponse
    {
        private string transactionId = string.Empty;
        private string languageCode = "en-US";
        private string machineName = System.Environment.MachineName;
        private long callDuration;
        private MOBWalletException exception;

        public string TransactionId
        {
            get
            {
                return this.transactionId;
            }
            set
            {
                this.transactionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string LanguageCode
        {
            get
            {
                return this.languageCode;
            }
            set
            {
                this.languageCode = string.IsNullOrEmpty(value) ? "en-US" : value.Trim();
            }
        }

        public string MachineName
        {
            get
            {
                return this.machineName;
            }
            set
            {
                this.machineName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public long CallDuration
        {
            get
            {
                return this.callDuration;
            }
            set
            {
                this.callDuration = value;
            }
        }

        public MOBWalletException Exception
        {
            get
            {
                return this.exception;
            }
            set
            {
                this.exception = value;
            }
        }
    }
}
