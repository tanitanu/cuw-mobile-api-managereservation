using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CommonDataListRequest : MOBRequest
    {
        public CommonDataListRequest()
            : base()
        {
        }
        private string sessionId = string.Empty;
        private string token = string.Empty;
        private bool getCountries;
        private bool getRewardPrograms;
        private bool getServiceAnimalResponses;
        private bool getServiceAnimalTypeResponses;
        private bool getSpecialRequestResponses;
        private bool getSuffixResponses;

        public string SessionId
        {
            get
            {
                return sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                this.token = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool GetCountries
        {
            get { return this.getCountries; }
            set { this.getCountries = value; }
        }

        public bool GetRewardPrograms
        {
            get { return this.getRewardPrograms; }
            set { this.getRewardPrograms = value; }
        }

        public bool GetServiceAnimalResponses
        {
            get { return this.getServiceAnimalResponses; }
            set { this.getServiceAnimalResponses = value; }
        }

        public bool GetServiceAnimalTypeResponses
        {
            get { return this.getServiceAnimalTypeResponses; }
            set { this.getServiceAnimalTypeResponses = value; }
        }

        public bool GetSpecialRequestResponses
        {
            get { return this.getSpecialRequestResponses; }
            set { this.getSpecialRequestResponses = value; }
        }

        public bool GetSuffixResponses
        {
            get { return this.getSuffixResponses; }
            set { this.getSuffixResponses = value; }
        }
    }
}
