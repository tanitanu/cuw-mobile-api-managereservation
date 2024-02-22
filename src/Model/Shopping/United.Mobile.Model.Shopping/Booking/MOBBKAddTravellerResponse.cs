using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
//using United.Reward.Configuration;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKAddTravellerResponse : MOBResponse
    {
        public MOBBKAddTravellerResponse()
        {
            rewardPrograms = new List<MOBBKRewardProgram>();
            ConfigurationParameter.ConfigParameter parm = ConfigurationParameter.ConfigParameter.Configuration;
            for (int i = 0; i < parm.RewardTypes.Count; i++)
            {
                MOBBKRewardProgram p = new MOBBKRewardProgram();
                p.Type = parm.RewardTypes[i].Type;
                p.Description = parm.RewardTypes[i].Description;
                rewardPrograms.Add(p);
            }
        }
        
        private string sessionId = string.Empty;

        private MOBBKAddTravellerRequest addTravellerRequest;

        private MOBBKTraveler currentTraveler;

        private List<MOBBKTraveler> travelers;

        private List<MOBBKRewardProgram> rewardPrograms;

        private bool isLastTraveler;

        private string phoneNumberDisclaimer = ConfigurationManager.AppSettings["PhoneNumberDisclaimer"];

        private List<MOBTypeOption> disclaimer = null;

        private long profileOwnerCustomerId;

        private string profileOwnerMPAccountNumber;

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBBKAddTravellerRequest AddTravellerRequest
        {
            get
            {
                return this.addTravellerRequest;
            }
            set
            {
                this.addTravellerRequest = value;
            }
        }

        public MOBBKTraveler CurrentTraveler
        {
            get { return this.currentTraveler; }
            set { this.currentTraveler = value; }
        }

        public List<MOBBKTraveler> Travelers
        {
            get { return this.travelers; }
            set { this.travelers = value; }
        }

        public List<MOBBKRewardProgram> RewardPrograms
        {
            get { return this.rewardPrograms; }
            set { this.rewardPrograms = value; }
        }

        public bool IsLastTraveler
        {
            get { return this.isLastTraveler; }
            set { this.isLastTraveler = value; }
        }

        public string PhoneNumberDisclaimer
        {
            get
            {
                return this.phoneNumberDisclaimer;
            }
            set
            {

            }
        }

        public long ProfileOwnerCustomerId
        {
            get { return this.profileOwnerCustomerId; }
            set { this.profileOwnerCustomerId = value; }
        }

        public string ProfileOwnerMPAccountNumber
        {
            get { return this.profileOwnerMPAccountNumber; }
            set { this.profileOwnerMPAccountNumber = value; }
        }

        public List<MOBTypeOption> Disclaimer
        {
            get { return disclaimer; }
            set { disclaimer = value; }
        }
    }
}
