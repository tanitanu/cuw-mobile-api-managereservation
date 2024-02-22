using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.ReShop
{
    [Serializable()]
    public class MOBRESHOPChangeEligibilityRequest : MOBModifyReservationRequest
    {
        private List<MOBShuttleOffer> shuttleOffer;
        private string recordLocator;
        private string lastName;
        private string flowType;
        private string checkinSessionKey;
        private string[] paxIndexes;
        private Boolean override24HrFlex;
        private Boolean overrideATREEligible;
        private MOBSHOPShopRequest reshopRequest;

        public MOBSHOPShopRequest ReshopRequest { get { return this.reshopRequest; } set { this.reshopRequest = value; } }
        public List<MOBShuttleOffer> ShuttleOffer { get { return this.shuttleOffer; } set { this.shuttleOffer = value; } }
        public string LastName
        { get { return lastName; } set { lastName = value; } }

        public string RecordLocator
        { get { return recordLocator; } set { recordLocator = value; } }

        public string FlowType
        { get { return this.flowType; } set { this.flowType = value; } }

        public string CheckinSessionKey
        { get { return this.checkinSessionKey; } set { this.checkinSessionKey = value; } }

        public string[] PaxIndexes { get { return this.paxIndexes; } set { this.paxIndexes = value; } }
        public Boolean Override24HrFlex { get { return this.override24HrFlex; } set { this.override24HrFlex = value; } }
        public Boolean OverrideATREEligible { get { return this.overrideATREEligible; } set { this.overrideATREEligible = value; } }
    }
}
