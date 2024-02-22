using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class CacheDataResponse
    {
        public CacheDataResponse()
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
