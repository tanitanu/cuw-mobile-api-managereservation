using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlightConfirmTravelerResponse :MOBResponse
    {
       
        public string SessionId { get; set; } = string.Empty;
       
        public FlightConfirmTravelerRequest FlightConfirmTravelerRequest { get; set; } 
        public MOBSHOPTraveler CurrentTraveler { get; set; } 

        public List<MOBSHOPTraveler> Travelers { get; set; } 


        public List<MOBSHOPTrip> Trips { get; set; } 

        public List<MOBCreditCard> CreditCards { get; set; } 
        public bool Finished { get; set; } 
        public bool IsLastTraveler { get; set; } 

        public List<MOBSeatMap> SeatMap { get; set; } 

        public List<MOBEmail> Emails { get; set; } 
        public List<MOBAddress> ProfileOwnerAddresses { get; set; } 

        public List<MOBTypeOption> ExitAdvisory { get; set; } 

        public List<TripSegment> Segments { get; set; } 
        public List<string> TermsAndConditions { get; set; } 

        public List<string> DOTBagRules { get; set; } 

        public string PhoneNumberDisclaimer { get; set; } = string.Empty;
       

        public List<MOBTypeOption> Disclaimer { get; set; }

        public List<RewardProgram> RewardPrograms { get; set; } 
        public MOBSHOPReservation Reservation { get; set; } 

        public List<MOBTypeOption> HazMat { get; set; }

        public string ContractOfCarriage { get; set; } = string.Empty;
        public FlightConfirmTravelerResponse()
            : base()
        {
            //this.rewardPrograms = new List<RewardProgram>();
            //ConfigurationParameter.ConfigParameter parm = ConfigurationParameter.ConfigParameter.Configuration;
            //for (int i = 0; i < parm.RewardTypes.Count; i++)
            //{
            //    RewardProgram p = new RewardProgram();
            //    p.Type = parm.RewardTypes[i].Type;
            //    p.Description = parm.RewardTypes[i].Description;
            //    rewardPrograms.Add(p);
            }
        
    }
}
