using Newtonsoft.Json;

namespace United.Mobile.Model.Internal
{
    public class DPSSOTokenRequest: DPTokenRequest

    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

    }
}
