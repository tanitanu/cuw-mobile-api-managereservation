using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ApplePayLoad
    {
        private string version = string.Empty;
        private string data = string.Empty;
        private string signature = string.Empty;

        private List<Dictionary<string, string>> additionalAppleInfo;

        public string Version
        {
            get
            {
                return version;
            }

            set
            {
                version = value;
            }
        }

        public string Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }

        public string Signature
        {
            get
            {
                return signature;
            }

            set
            {
                signature = value;
            }
        }

        private Header header;

        public List<Dictionary<string, string>> AdditionalAppleInfo
        {
            get
            {
                return additionalAppleInfo;
            }

            set
            {
                additionalAppleInfo = value;
            }
        }

        public Header Header
        {
            get
            {
                return header;
            }

            set
            {
                header = value;
            }
        }
    }

    [Serializable()]
    public class Header
    {
        private string ephemeralPublicKey = string.Empty;
        private string publicKeyHash = string.Empty;
        private string transactionId = string.Empty;

        public string EphemeralPublicKey
        {
            get
            {
                return ephemeralPublicKey;
            }

            set
            {
                ephemeralPublicKey = value;
            }
        }

        public string PublicKeyHash
        {
            get
            {
                return publicKeyHash;
            }

            set
            {
                publicKeyHash = value;
            }
        }

        public string TransactionId
        {
            get
            {
                return transactionId;
            }

            set
            {
                transactionId = value;
            }
        }
    }
}
