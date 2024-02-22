using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class InsertCacheDataRequest
    {
        public InsertCacheDataRequest()
            : base()
        {
        }


        private int intID;
        private string strGUID;
        private string strDeviceID;
        private string strMPNumber;
        private int intAppId;
        private string strAppVersion;
        private string strCacheData;
        private string strDataDescription;

        public int IntID
        {
            get
            {
                return this.intID;
            }
            set
            {
                this.intID = value;
            }
        }
        public string StrGUID
        {
            get
            {
                return this.strGUID;
            }
            set
            {
                this.strGUID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string StrDeviceID
        {
            get
            {
                return this.strDeviceID;
            }
            set
            {
                this.strDeviceID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string StrMPNumber
        {
            get
            {
                return this.strMPNumber;
            }
            set
            {
                this.strMPNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public int IntAppId
        {
            get
            {
                return this.intAppId;
            }
            set
            {
                this.intAppId = value;
            }
        }
        public string StrAppVersion
        {
            get
            {
                return this.strAppVersion;
            }
            set
            {
                this.strAppVersion = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string StrCacheData
        {
            get
            {
                return this.strCacheData;
            }
            set
            {
                this.strCacheData = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string StrDataDescription
        {
            get
            {
                return this.strDataDescription;
            }
            set
            {
                this.strDataDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
