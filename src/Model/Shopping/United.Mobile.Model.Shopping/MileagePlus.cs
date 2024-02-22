using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MileagePlus
    {
       
        public int AccountBalance { get; set; } 

        public string ActiveStatusCode { get; set; } = string.Empty;
        
        public string ActiveStatusDescription { get; set; } = string.Empty;
        
        public int AllianceEliteLevel { get; set; } 

        public string ClosedStatusCode { get; set; } = string.Empty;
      
        public string ClosedStatusDescription { get; set; } = string.Empty;
      
        public int CurrentEliteLevel { get; set; } 
        public string CurrentEliteLevelDescription { get; set; } = string.Empty;
       
        public decimal CurrentYearMoneySpent { get; set; } 

        public int EliteMileageBalance { get; set; } 

        public int EliteSegmentBalance { get; set; } 

        public int EliteSegmentDecimalPlaceValue { get; set; } 
        
        public string EncryptedPin { get; set; } = string.Empty;
       
        public string EnrollDate { get; set; } = string.Empty;
       
        public string EnrollSourceCode { get; set; } = string.Empty;
        
        public string EnrollSourceDescription { get; set; } = string.Empty;
       
        public int FlexEqmBalance { get; set; } 

        public int FutureEliteLevel { get; set; } 
        public string FutureEliteDescription { get; set; } = string.Empty;
       
        public string InstantEliteExpirationDate { get; set; } = string.Empty;
      
        public bool IsCEO { get; set; }
        public bool IsClosedPermanently { get; set; } 

        public bool IsClosedTemporarily { get; set; } 
        public bool IsCurrentTrialEliteMember { get; set; } 
        public bool IsFlexEqm { get; set; } 

        public bool IsInfiniteElite { get; set; } 
        public bool IsLifetimeCompanion { get; set; }

        public bool IsLockedOut { get; set; } 
        public bool IsMergePending { get; set; } 
        public bool IsUnitedClubMember { get; set; } 
        public bool IsPresidentialPlus { get; set; } 
        public string Key { get; set; } = string.Empty;
      
        public string LastActivityDate { get; set; } = string.Empty;
       
        public int LastExpiredMile { get; set; } 
        public string LastFlightDate { get; set; } = string.Empty;
       
        public int LastStatementBalance { get; set; } 

        public string LastStatementDate { get; set; } = string.Empty;
    
        public int LifetimeEliteLevel { get; set; } 
        public int LifetimeEliteMileageBalance { get; set; } 
        public string MileageExpirationDate { get; set; } = string.Empty;
      
        public int NextYearEliteLevel { get; set; } 

        public string NextYearEliteLevelDescription { get; set; } = string.Empty;
       
        public string MileagePlusId { get; set; } = string.Empty;
       
        public string MileagePlusPin { get; set; } = string.Empty;
       
        public string PriorUnitedAccountNumber { get; set; } = string.Empty;
        
        public int SkyTeamEliteLevel { get; set; } 
    }
}
