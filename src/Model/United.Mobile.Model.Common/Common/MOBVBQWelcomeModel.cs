using Microsoft.Extensions.Configuration;
using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class VBQWelcomeModel
    {
        private readonly IConfiguration _configuration;

        public VBQWelcomeModel()
        {
        }

        public VBQWelcomeModel(IConfiguration configuration)
        {
            title = _configuration.GetValue<string>("VBQModelTitle");
            description = _configuration.GetValue<string>("VBQModelDescription");
            stringThanks = _configuration.GetValue<string>("VBQModelThanksText");
            stringLearnMore = _configuration.GetValue<string>("VBQModelLearnMoreText");
            linkLearnMore = _configuration.GetValue<string>("VBQModelLearnMoreHyperLink");
        }
        public string title { get; set; }
        public string description { get; set; } 
        public string stringThanks { get; set; }
        public string stringLearnMore { get; set; }
        public string linkLearnMore { get; set; }

    }
}
