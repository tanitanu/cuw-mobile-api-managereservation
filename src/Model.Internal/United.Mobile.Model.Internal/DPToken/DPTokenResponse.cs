using Newtonsoft.Json;

namespace United.Mobile.Model.Internal
{
    public class DPTokenResponse
    {
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty(PropertyName = "consented_on")]
        public int ConsentedOn { get; set; }
        [JsonProperty(PropertyName = "id_token")]
        public string IdToken { get; set; }
    }
    public class ActiveTokenResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "consented_on")]
        public long ConsentedOn
        {
            get;
            set;
        }


        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn
        {
            get;
            set;
        }


        [JsonProperty(PropertyName = "grant_type")]
        public string GrantType
        {
            get;
            set;
        }


        [JsonProperty(PropertyName = "id_token")]
        public string IdToken
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "refresh_token_expires_in")]
        public int? RefreshTokenExpiresIn
        {
            get;
            set;
        }


        [JsonProperty(PropertyName = "scope")]
        public string Scope
        {
            get;
            set;
        }


        [JsonProperty(PropertyName = "token_type")]
        public string TokenType
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "error")]
        public string Error
        {
            get;
            set;
        }


        [JsonProperty(PropertyName = "error_description")]
        public string ErrorDescription
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "active")]
        public bool Active
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "status")]
        public string Status
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "valid")]
        public string Valid
        {
            get;
            set;
        }
    }
}