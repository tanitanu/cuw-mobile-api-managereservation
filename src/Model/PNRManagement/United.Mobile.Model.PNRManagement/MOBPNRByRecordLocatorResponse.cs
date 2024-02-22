using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Definition.Accelerators;
using United.Definition.Pcu;
using United.Definition.Shopping;
using United.Definition.Shopping.TripInsurance;
using United.Definition.SSR;

namespace United.Mobile.Model.PNRManagement
{
    [Serializable()]
    public class MOBPNRByRecordLocatorResponse : MOBResponse
    {
        private string deviceId = string.Empty;
        private string sessionId = string.Empty;
        private string flow = string.Empty;
        private string recordLocator = string.Empty;
        private string lastName = string.Empty;
        private MOBPNR pnr;
        private string uaRecordLocator = string.Empty;
        private List<string> dotBagRules;
        private MOBDOTBaggageInfo dotBaggageInformation;
        private MOBPremierAccess premierAccess;
        private List<MOBSHOPRewardProgram> rewardPrograms;
        private MOBTravelSpecialNeeds specialNeeds;
        private bool showSeatChange;
        private bool showPremierAccess;
        private bool showAddCalendar = true;
        private bool showBaggageInfo = true;
        // All properties here should also be in
        // MOBPremierAccessCompleteResponse,
        // since iOS is using MOBPNRByRecordLocatorResponse as CompletePremierAccessSelectionECC
        // response model. 
        private MOBTPIInfo tripInsuranceInfo;
        private MOBAncillary ancillary;

        private bool showJoinOneClickEnrollment = false;

        public bool ShowJoinOneClickEnrollment
        {
            get { return this.showJoinOneClickEnrollment; }
            set { this.showJoinOneClickEnrollment = value; }
        }

        public string DeviceId
        {
            get
            {
                return this.deviceId;
            }
            set
            {
                this.deviceId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public MOBTravelSpecialNeeds SpecialNeeds
        {
            get { return this.specialNeeds; }
            set { this.specialNeeds = value; }
        }
        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Flow
        {
            get
            {
                return this.flow;
            }
            set
            {
                this.flow = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public List<MOBSHOPRewardProgram> RewardPrograms { get { return this.rewardPrograms; } set { this.rewardPrograms = value; } }
        public string RecordLocator
        {
            get
            {
                return this.recordLocator;
            }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }


        public string UARecordLocator
        {
            get
            {
                return this.uaRecordLocator;
            }
            set
            {
                this.uaRecordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public MOBPNR PNR
        {
            get
            {
                return this.pnr;
            }
            set
            {
                this.pnr = value;
            }
        }

        public List<string> DOTBagRules
        {
            get
            {
                string rText = System.Configuration.ConfigurationManager.AppSettings["DOTBagRules"];
                if (!string.IsNullOrEmpty(rText))
                {
                    string[] rules = rText.Split('|');
                    if (rules != null && rules.Length > 0)
                    {
                        this.dotBagRules = new List<string>();
                        foreach (string s in rules)
                        {
                            this.dotBagRules.Add(s);
                        }
                    }
                }

                return this.dotBagRules;
            }
            set
            {
                this.dotBagRules = value;
            }
        }

        public MOBDOTBaggageInfo DotBaggageInformation
        {
            get
            {
                return this.dotBaggageInformation;
            }
            set
            {
                this.dotBaggageInformation = value;
            }
        }

        public bool ShowSeatChange
        { get { return this.showSeatChange; } set { this.showSeatChange = value; } }

        public bool ShowPremierAccess
        { get { return this.showPremierAccess; } set { this.showPremierAccess = value; } }

        public bool ShowAddCalendar
        { get { return this.showAddCalendar; } set { this.showAddCalendar = value; } }

        public bool ShowBaggageInfo
        { get { return this.showBaggageInfo; } set { this.showBaggageInfo = value; } }


        public MOBPremierAccess PremierAccess
        {
            get
            {
                return this.premierAccess;
            }
            set
            {
                this.premierAccess = value;
            }
        }

        public MOBTPIInfo TripInsuranceInfo
        {
            get
            {
                return this.tripInsuranceInfo;
            }
            set
            {
                this.tripInsuranceInfo = value;
            }
        }

        public MOBAncillary Ancillary
        {
            get { return ancillary; }
            set { ancillary = value; }
        }

      

    }

   



    [Serializable()]
    public class MOBCancelFFCPNRsByMPNumberResponse : MOBResponse
    {
        private string mileagePlusNumber;
        private bool futureFlightCreditLink;
        public bool FutureFlightCreditLink
        {
            get { return futureFlightCreditLink; }
            set { futureFlightCreditLink = value; }
        }

        private List<MOBCancelledFFCPNRDetails> cancelledFFCPNRList;
        public List<MOBCancelledFFCPNRDetails> CancelledFFCPNRList
        {
            get { return cancelledFFCPNRList; }
            set { cancelledFFCPNRList = value; }
        }
        public string MileagePlusNumber
        {
            get { return mileagePlusNumber; }
            set { mileagePlusNumber = value; }
        }
    }
    [Serializable()]
    public class MOBCancelledFFCPNRDetails
    {
        private string recordLocator;
        public string RecordLocator
        {
            get { return recordLocator; }
            set { recordLocator = value; }
        }

        private string pnrLastName;

        public string PNRLastName
        {
            get { return pnrLastName; }
            set { pnrLastName = value; }
        }

        private List<MOBName> passengers;
        public List<MOBName> Passengers
        {
            get { return passengers; }
            set { passengers = value; }
        }
    }

    [Serializable()]
    public class MOBCancelFFCPNRsByMPNumberRequest : MOBRequest
    {
        private string mileagePlusNumber = string.Empty;
        private string sessionId = string.Empty;
        private string hashValue;

        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public string HashValue
        {
            get { return hashValue; }
            set { hashValue = value; }
        }
    }
}
