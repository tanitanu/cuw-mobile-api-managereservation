using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Shopping.TripInsurance
{
    [Serializable]
    public class TPIPurchaseInViewResRequest: MOBRequest
    {
       
        public string SessionId { get; set; } = string.Empty;
        
        public string Cid { get; set; } = string.Empty;
       
        public string CcHolderName { get; set; } = string.Empty;
       
        public string CcCode { get; set; } = string.Empty;
        
        public string ExpMonth { get; set; } = string.Empty;
        
        public string ExpYear { get; set; } = string.Empty;
       

        public string EmailAddress { get; set; } = string.Empty;
       

        public MOBAddress Address { get; set; } 
       
       
        public string CardNumber { get; set; } = string.Empty;
       

        public string EccNumber { get; set; } = string.Empty;
        

        public string CcType { get; set; } = string.Empty;
      

        public string RecordLocator { get; set; } = string.Empty;
       
        public string LastName { get; set; } = string.Empty;
        
    }
}
