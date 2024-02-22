using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Common;
using United.Mobile.Model.MPSignIn;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.ManageRes
{
    [Serializable()]
    public class MOBPNRByRecordLocatorResponse : MOBResponse
    {

        public MOBPNRByRecordLocatorResponse()
        {

        }



        private IConfiguration _configuration;
        private string deviceId = string.Empty;
        private string sessionId = string.Empty;
        private string flow = string.Empty;
        private string recordLocator = string.Empty;
        private string lastName = string.Empty;
        private MOBPNR pnr;
        private string uaRecordLocator = string.Empty;
        private List<string> dotBagRules;
        private DOTBaggageInfo dotBaggageInformation;
        private MOBCountDownWidgetInfo countDownWidgetInfo;
        private MOBPremierAccess premierAccess;
        private List<RewardProgram> rewardPrograms;
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
        private MOBTravelSpecialNeeds specialNeeds;
        private MOBShareReservationInfo shareReservationInfo;
        private TravelOptions travelOptions;
        private MOBAddPetEligible petEligibility;

        public MOBPNRByRecordLocatorResponse(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public MOBShareReservationInfo ShareReservationInfo { get { return this.shareReservationInfo; } set { this.shareReservationInfo = value; } }

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

        public List<RewardProgram> RewardPrograms { get { return this.rewardPrograms; } set { this.rewardPrograms = value; } }
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
                if (_configuration != null)
                {
                    string rText = _configuration.GetValue<string>("DOTBagRules");
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
                }
                return this.dotBagRules;
            }
            set
            {
                this.dotBagRules = value;
            }
        }


        public void DOTBagrules(IConfiguration configuration)
        {
            _configuration = configuration;

            dotBagRules = new List<string> { }; string rText = _configuration.GetValue<string>("DOTBagRules");

            if (!string.IsNullOrEmpty(rText)) { string[] rules = rText.Split('|'); if (rules != null && rules.Length > 0) { this.dotBagRules = new List<string>(); foreach (string s in rules) { this.dotBagRules.Add(s); } } }
        }



        public DOTBaggageInfo DotBaggageInformation
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

        public MOBCountDownWidgetInfo CountDownWidgetInfo
        {
            get
            {
                return this.countDownWidgetInfo;
            }
            set
            {
                this.countDownWidgetInfo = value;
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
        public TravelOptions TravelOptions
        {
            get { return travelOptions; }
            set { travelOptions = value; }
        }

        private MOBSustainableAviationFuel sustainableAviationFuel;

        public MOBSustainableAviationFuel SustainableAviationFuel
        {
            get { return sustainableAviationFuel; }
            set { sustainableAviationFuel = value; }
        }
        public MOBAddPetEligible PetEligibility
        {
            get { return petEligibility; }
            set { petEligibility = value; }
        }

    }

}
