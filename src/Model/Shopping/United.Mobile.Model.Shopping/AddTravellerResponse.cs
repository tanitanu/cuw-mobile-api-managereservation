using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
//using United.Reward.Configuration;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class AddTravellerResponse //:MOBResponse
    {
        public AddTravellerResponse()
        {
            RewardPrograms = new List<RewardProgram>();
            //ConfigurationParameter.ConfigParameter parm = ConfigurationParameter.ConfigParameter.Configuration;
            //for (int i = 0; i < parm.RewardTypes.Count; i++)
            //{
            //    RewardProgram p = new RewardProgram();
            //    p.Type = parm.RewardTypes[i].Type;
            //    p.Description = parm.RewardTypes[i].Description;
            //    rewardPrograms.Add(p);
            //}
            Travelers = new List<MOBSHOPTraveler>();
            RewardPrograms = new List<RewardProgram>();
            Disclaimer = new List<MOBTypeOption>();
        }
      
        public string SessionId { get; set; } = string.Empty;
        

        public AddTravellerRequest AddTravellerRequest { get; set; }
           

        public MOBSHOPTraveler CurrentTraveler { get; set; } 
          

        public List<MOBSHOPTraveler> Travelers { get; set; } 


        public List<RewardProgram> RewardPrograms { get; set; } 


        public bool IsLastTraveler { get; set; } 


        public string PhoneNumberDisclaimer { get; set; } = string.Empty;


        public long ProfileOwnerCustomerId { get; set; } 


        public string ProfileOwnerMPAccountNumber { get; set; } = string.Empty;


        public List<MOBTypeOption> Disclaimer { get; set; } 
    }
}
