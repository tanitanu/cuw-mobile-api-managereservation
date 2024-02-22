using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.TripInsurance
{
    [Serializable]
    public class RegisterOfferForTPIRequest : MOBRequest
    {
        public string SessionId { get; set; } = string.Empty;
        
        public bool IsRegisterOffer { get; set; } 
       
        public bool IsReQuote { get; set; } 
        public bool IsAcceptChanges { get; set; } 
    }
}
