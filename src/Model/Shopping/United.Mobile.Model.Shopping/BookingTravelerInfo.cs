using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class BookingTravelerInfo
    {
       
        public string Key { get; set; } = string.Empty;
        

        public MOBName TravelerName { get; set; } 
      

        public List<SecureTraveler> SsrName { get; set; } 


        public List<Phone> Phones { get; set; } 

        public string PaxIndex { get; set; } = string.Empty;
       
        public bool IsProfileOwner { get; set; } 


        public string TravelerTypeCode { get; set; } = string.Empty;
        

        public string DateOfBirth { get; set; } = string.Empty;
       

        public string CustomerId { get; set; } = string.Empty;
       

        public string IdType { get; set; } = string.Empty;
        

        public string Gender { get; set; } = string.Empty;
       
        /*
        private string requestedSeat = string.Empty;

        public string RequestedSeat
        {
            get { return requestedSeat; }
            set { requestedSeat = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }
        */
      
        public string AccountNumber { get; set; } = string.Empty;
      
        public string TravelProgram { get; set; } = string.Empty;
        
        public List<AirRewardProgram> AirRewardProgram { get; set; }

        public int CurrentEliteLevel { get; set; } 

        public EliteStatus EliteStatus { get; set; } 

        public bool IsGoldEliteAsEPA { get; set; }

        public string EPAMessageTitle { get; set; } = string.Empty;
       

        public string EPAMessage { get; set; } = string.Empty;
      
        public Ticket Ticket { get; set; }

        public bool IsTSAFlagON { get; set; } 

        public string FQTVNameMisMatchText { get; set; } 
      
        public bool ISFQTVMadeEmptyForNameMisMatch { get; set; } 

        private string nameMisMatchFQTVNumber = string.Empty;

        public string NameMisMatchFQTVNumber { get; set; } = string.Empty;
      
        public List<MOBPrefAirPreference> AirPreferences { get; set; } 


        public List<PrefContact> Contacts { get; set; } 

        
        public bool IsEPlusSubscriber { get; set; } 

        public MOBUASubscriptions EPlusSubscriptions { get; set; }
        public BookingTravelerInfo()
        {
            SsrName = new List<SecureTraveler>();
            Phones = new List<Phone>();
            AirRewardProgram = new List<AirRewardProgram>();
            AirPreferences = new List<MOBPrefAirPreference>();
            Contacts = new List<PrefContact>();
        }
    }
}
