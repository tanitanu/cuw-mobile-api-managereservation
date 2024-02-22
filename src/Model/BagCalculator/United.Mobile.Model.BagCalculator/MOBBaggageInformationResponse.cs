using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.BagCalculator
{
    public class MOBBaggageInformationResponse : MOBResponse
    {
        public MOBBaggageInformationResponse(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private string sessionId;
        private string flow;
        private List<string> dotBagRules;
        private DOTBaggageInfo dotBaggageInformation;
        private readonly IConfiguration _configuration;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }
        public string Flow
        {
            get { return flow; }
            set { flow = value; }
        }
        public DOTBaggageInfo DotBaggageInformation
        {
            get
            {
                return this.dotBaggageInformation;
            }
            set
            {
                this.dotBaggageInformation = value;
            }
        }

        public List<string> DOTBagRules
        {
            get
            {
                string rText = _configuration.GetValue<string>("DOTBagRules");
                if (!string.IsNullOrEmpty(rText))
                {
                    string[] rules = rText.Split('|');
                    if (rules != null && rules.Length > 0)
                    {
                        this.dotBagRules = new List<string>();
                        foreach (string s in rules)
                        {
                            this.dotBagRules.Add(s);
                        }
                    }
                }

                return this.dotBagRules;
            }
            set
            {
                this.dotBagRules = value;
            }
        }
    }
}
