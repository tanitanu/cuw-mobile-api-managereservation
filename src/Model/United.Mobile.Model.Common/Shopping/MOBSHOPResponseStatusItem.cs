using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using United.Mobile.Model.Common;

namespace United.Definition
{
    public class MOBSHOPResponseStatusItem
    {
        private MOBSHOPResponseStatus status;
        public MOBSHOPResponseStatus Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        private List<MOBItem> statusMessages;
        public List<MOBItem> StatusMessages
        {
            get { return statusMessages; }
            set { statusMessages = value; }
        }
    }
    [Serializable]
    public enum MOBSHOPResponseStatus
    {
        [EnumMember(Value = "1")]
        ReshopUnableToComplete,
        [EnumMember(Value = "2")]
        ReshopChangePending,
        [EnumMember(Value = "3")]
        ReshopBENonElgible,
        [EnumMember(Value = "4")]
        ReshopUnableToChange,
        [EnumMember(Value = "5")]
        PcuUpgradeFailed,
        [EnumMember(Value = "6")]
        FailedToGetBagChargeInfo,
        [EnumMember(Value = "7")]
        ReshopAgencyCheckinEligible,
        [EnumMember(Value = "8")]
        ReshopCheckinEligible,
        [EnumMember(Value = "9")]
        ReshopChangeOfferBEBuyOut,
        [EnumMember(Value = "10")]
        ReshopOTFShopEligible,
    }
}
