using System;

namespace United.Mobile.Model.Common.DPToken
{
    public class TokenData
    {
        public string Token { get; set; }
        public TimeSpan TokenExpiration { get; set; }
        public DateTime TokenExpirationDateTimeUtc { get; set; }
    }
}
