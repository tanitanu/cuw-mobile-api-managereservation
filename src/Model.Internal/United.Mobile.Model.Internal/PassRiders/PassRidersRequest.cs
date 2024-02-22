using United.Mobile.Model.Internal.Common;
namespace United.Mobile.Model.Internal.PassRiders
{
    public class PassRidersRequest : EResBaseRequest
    {
        public bool Customize { get; set; }
        public string TravelType { get; set; }
        public string SessionId { get; set; } 
    }
}
