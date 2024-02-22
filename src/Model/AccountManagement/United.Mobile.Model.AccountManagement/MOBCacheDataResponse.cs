using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPSignIn
{
    [Serializable]
    public class MOBCacheDataResponse
    {
        public MOBCacheDataResponse()
            : base()
        {
        }

        private int id;
        private bool blnRefresh;
        private DateTime lastUpdateDateTime;
        private string cacheData;

        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public bool BlnRefresh
        {
            get
            {
                return this.blnRefresh;
            }
            set
            {
                this.blnRefresh = value;
            }
        }
        public DateTime LastUpdateDateTime
        {
            get
            {
                return this.lastUpdateDateTime;
            }
            set
            {
                this.lastUpdateDateTime = value;
            }
        }
        public string CacheData
        {
            get
            {
                return this.cacheData;
            }
            set
            {
                this.cacheData = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
