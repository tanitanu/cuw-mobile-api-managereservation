using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBCorporateTravelTypeItem
    {
        private string travelType;
        private string travelTypeDescription;
        private TravelPolicy travelPolicy;

        public string TravelType
        {
            get
            {
                return travelType;
            }
            set
            {
                travelType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TravelTypeDescription
        {
            get
            {
                return travelTypeDescription;
            }
            set
            {
                travelTypeDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public TravelPolicy TravelPolicy
        {
            get
            {
                return travelPolicy;
            }
            set
            {
                travelPolicy = value;
            }
        }
    }

    [Serializable]
    public class TravelPolicy
    {
        private string travelPolicyTitle;
        private string travelPolicyHeader;
        private string travelPolicyBody;
        private List<MOBSection> travelPolicyContent;
        private string travelPolicyFooterMessage;
        private List<string> travelPolicyButton;

        public string TravelPolicyTitle
        {
            get
            {
                return travelPolicyTitle;
            }
            set
            {
                travelPolicyTitle = value;
            }
        }

        public string TravelPolicyHeader
        {
            get
            {
                return travelPolicyHeader;
            }
            set
            {
                travelPolicyHeader = value;
            }
        }

        public string TravelPolicyBody
        {
            get
            {
                return travelPolicyBody;
            }
            set
            {
                travelPolicyBody = value;
            }
        }

        public List<MOBSection> TravelPolicyContent
        {
            get
            {
                return travelPolicyContent;
            }
            set
            {
                travelPolicyContent = value;
            }
        }

        public string TravelPolicyFooterMessage
        {
            get
            {
                return travelPolicyFooterMessage;
            }
            set
            {
                travelPolicyFooterMessage = value;
            }
        }

        public List<string> TravelPolicyButton
        {
            get
            {
                return travelPolicyButton;
            }
            set
            {
                travelPolicyButton = value;
            }
        }
    }
}
