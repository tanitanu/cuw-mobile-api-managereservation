using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.BagCalculator
{
    [Serializable]
    public class DOTBaggageAdditionalDetails
    {
        private readonly IConfiguration _configuration;
        public string Title1 { get; set; } = string.Empty;
        public string Title2 { get; set; } = string.Empty;
        public string FreeBagsHeaderText { get; set; } = string.Empty;
        public string FreeBagsDescriptionText { get; set; } = string.Empty;
        public string OACarrierText { get; set; } = string.Empty;
        public BagFeesPerSegment BaggageFeesPerSegment { get; set; }
        public string AdditionalOtherBagFeesTitle { get; set; } = string.Empty;
        public List<AdditionalBagDetails> AdditionalAndOverSizeOverWeightBagDetails { get; set; }
        public string AdditionalOtherBagFeesNote { get; set; } = string.Empty;
        public string ChaseCardFreeFirstBagHeaderText { get; set; } = string.Empty;
        public string ChaseCardFreeFirstBagDescriptionText { get; set; } = string.Empty;
        public string DefaultCheckInBagDimensions { get; set; } = string.Empty;
        public BagFeesPerSegment IBeLiteBaggageFeesPerSegment { get; set; }
        public BagFeesPerSegment IBeBaggageFeesPerSegment { get; set; }
    
        public DOTBaggageAdditionalDetails(bool getDOTStaticInfoText, IConfiguration configuration)
        {
            if (getDOTStaticInfoText)
            {
                Title1 = configuration.GetValue<string>("DOTBaggageAdditionalDetailsTitle1").Split('|')[0];
                Title2 = configuration.GetValue<string>("DOTBaggageAdditionalDetailsTitle2").Split('|')[0];

                AdditionalOtherBagFeesTitle = configuration.GetValue<string>("AdditionalOtherBagFeeTitle").Split('|')[0];
                AdditionalOtherBagFeesNote = configuration.GetValue<string>("AdditionalOtherBagFeeNote").Split('|')[0];
                DefaultCheckInBagDimensions = configuration.GetValue<string>("DefaultCheckInBagDimensions").Split('|')[0];
            }
        }
    }
}
