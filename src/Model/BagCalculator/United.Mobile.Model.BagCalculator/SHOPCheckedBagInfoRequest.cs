using United.Mobile.Model.Common;

namespace United.Mobile.Model.BagCalculator
{
    public class SHOPCheckedBagInfoRequest : MOBRequest
    {       
        public string LoyaltyLevel { get; set; }
        public string SessionId { get; set; }
        public string CartId { get; set; }
    }   
    
}
