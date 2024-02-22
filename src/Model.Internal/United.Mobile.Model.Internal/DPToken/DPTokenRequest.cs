using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Internal
{
    public class DPTokenRequest
    {
        [JsonProperty(PropertyName = "client_id")]
        public string ClientId { get; set; }
        [JsonProperty(PropertyName = "client_secret")]
        public string ClientSecret { get; set; }
        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }
        [JsonProperty(PropertyName = "userType")]
        public string UserType { get; set; }
        [JsonProperty(PropertyName = "grant_type")]
        public string GrantType { get; set; }
        public string Nonce { get; set; }
        public string PrevAccessToken { get; set; }
        public string endUserAgentIP { get; set; }
        public string endUserAgentID { get; set; }
        public string application_id { get; set; }
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
    }
}
