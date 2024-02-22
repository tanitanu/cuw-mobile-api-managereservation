using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBVormetricKeys
    {
        private string accountNumberToken = string.Empty;
        private string persistentToken = string.Empty;
        private string securityCodeToken = string.Empty;
        private string cardType = string.Empty;

        public string AccountNumberToken
        {
            get
            {
                return this.accountNumberToken;
            }
            set
            {
                this.accountNumberToken = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PersistentToken
        {
            get
            {
                return this.persistentToken;
            }
            set
            {
                this.persistentToken = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string SecurityCodeToken
        {
            get
            {
                return this.securityCodeToken;
            }
            set
            {
                this.securityCodeToken = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CardType
        {
            get
            {
                return this.cardType;
            }
            set
            {
                this.cardType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
