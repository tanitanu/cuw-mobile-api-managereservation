using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using United.ile.Model.BagCalculator;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.BagCalculator
{
    [Serializable]
    public class DOTBaggageCalculatorResponse : MOBResponse
    {
        public DOTBaggageInfoRequest Request { get; set; }
        public List<DOTBaggageAdditionalDetails> CheckedAndOtherBagFees { get; set; }
        public string faqsCheckedBagsTitle { get; set; }
        public List<DOTBaggageFAQ> BaggageFAQs { get; set; }
        public string MyCheckedBagServiceChargesDesc { get; set; } = string.Empty;
        public bool IsAnyFlightSearch { get; set; }
        public DOTBaggageCalculatorResponse()
        {
            CheckedAndOtherBagFees = new List<DOTBaggageAdditionalDetails>();
            BaggageFAQs = new List<DOTBaggageFAQ>();
        }
        public DOTBaggageCalculatorResponse(bool getDOTStaticInfoText, IConfiguration configuration)
        {
            if (getDOTStaticInfoText)
            {
                faqsCheckedBagsTitle = configuration.GetValue<string>("DOTBaggageFAQsTitle").Split('|')[0].ToString();
                MyCheckedBagServiceChargesDesc = configuration.GetValue<string>("MyCheckedBagServiceChargesDesc").Split('|')[0].ToString();
            }
        }
    }
}
