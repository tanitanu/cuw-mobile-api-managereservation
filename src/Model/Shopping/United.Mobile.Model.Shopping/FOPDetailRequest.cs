using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class FOPDetailRequest : MOBRequest
    {
        public MOBSHOPFormOfPayment FormOfPaymentType { get; set; } 
        public string MPNumber { get; set; } = string.Empty;
       
        public string CustomerId { get; set; } = string.Empty;
        
        public string SessionId { get; set; } = string.Empty;
        
    }
}
