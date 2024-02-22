using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ChasePromoRedirectResponse:MOBResponse
    {
        public string webSessionShareURL { get; set; } = string.Empty;
        public string redirectURL { get; set; } = string.Empty;
        public string returnURL { get; set; } = string.Empty;
        public List<string> ReturnURLs { get; set; }
        public string Token { get; set; } = string.Empty;
        public ChasePromoRedirectResponse()
        {
            ReturnURLs = new List<string>();
        }
    }
}