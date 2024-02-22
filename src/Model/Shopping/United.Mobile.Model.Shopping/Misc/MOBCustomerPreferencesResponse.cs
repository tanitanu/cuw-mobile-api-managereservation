using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CustomerPreferencesResponse : MOBResponse
    {
        private string sessionId = string.Empty;
        private bool isExpertModeEnabled;
        private string mileagePlusNumber;


        public string MileagePlusNumber
        {
            get { return this.mileagePlusNumber; }
            set { this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string SessionId
        {
            get { return this.sessionId; }
            set { this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public bool IsExpertModeEnabled
        {
            get { return this.isExpertModeEnabled; }
            set { this.isExpertModeEnabled = value; }
        }
    }
}
