using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.BagCalculator
{
    [Serializable()]
    public class DOTCheckedBagCalculatorResponse : MOBResponse
    {
        public DOTCheckedBagCalculatorRequest Request { get; set; }
        public string PageTitle { get; set; }
        public string VariantInfoMessage { get; set; }
        public string FeeCollectMessage { get; set; }
        public string ConfirmationNumMessage { get; set; }
        public BagSizeAndWeightLimits BagSizeAndWeightLimits { get; set; }
        public AdditionalMeasuresBagFeeDetails AdditionalMeasuresBagFeeDetails { get; set;}
        public List<BaggageFeesPerSegment> ListBaggageFeesPerSegment { get; set; }
        public CardBenefitMessage CardBenefitMessage { get; set; }
        public string FooterText { get; set; }
        public string AdditionalBagfareDetailsButtonTitle { get; set; }
        public string AdditionalBagLimitsButtonTitle { get; set; }
        public string FaqButtonTitle { get; set; }
        public Faq Faq { get; set; }
        public bool IsAnyFlightSearch { get; set; }
        public string PrepayBagButtonText { get; set; }
        public string PrepayBagButtonUrl { get; set; }

        public string CartId { get; set; }

        public string SessionId { get; set; }

        public MemberShipStatus LoyaltyLevelSelected { get; set; }

        public List<MemberShipStatus> LoyaltyLevels { get; set; }

        public List<MOBItem> Captions { get; set; }

    }

    public class Faq
    {
        public string PageTitle { get; set; }
        public List<FaqMessage> ListFaqMessages { get; set; }
    }


    public class FaqMessage
    {
        public string Header { get; set; }
        public List<QuestionAndAnswers> ListQuestionAndAnswers { get; set; }
    }

    public class QuestionAndAnswers
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    public class CardBenefitMessage
    {
        public string ImageUrl { get; set; }
        public string Header { get; set; }

        public string Message { get; set; }

        public string ButtonText { get; set; }
        public string ButtonLink { get; set; }

    }

    public class BaggageFeesPerSegment
    {
        public string CabinName { get; set; }
        public string subTitle { get; set; }

        public CheckedBagEligibilityInfo CheckedBagEligibilityInfo { get; set; }

        public List<FareItems> ListFareItems { get; set; }

        public string DiscountRuleMessage { get; set; }

        public string DefaultCheckInBagDimensionsMessage { get; set; }

        public string DateInfoMessage { get; set; }

        public string OriginDestinationInfoMessage { get; set; }
    }

    public class FareItems
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string MessageText { get; set; }

        public string AccessibilityMessage { get; set; }

        public string OriginalFareMessageText { get; set; }

        public string AccessibilityOriginalFare { get; set; }

        public bool IsDiscountedItem { get; set; }

        public bool IsFree { get; set; }
        public bool IsFaredItem { get; set; }
        public bool HideDivider { get; set; }

    }

    public class CheckedBagEligibilityInfo
    {
        public string ImageUrl { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }

    }

    public class AdditionalMeasuresBagFeeDetails
    {
        public string PageTitle { get; set; }
        public List<AdditionalBagDetails> AdditionalAndOverSizeOverWeightBagDetails { get; set; }
        public string MeasuresCautionMessage { get; set; }
    }

    public class BagSizeAndWeightLimits
    {
        public string PageTitle { get; set; }
        public string LimitsMessage { get; set; }
        public List<WeightLimits> ListWeightLimits { get; set; }

        public string AdditionalServiceChargesMessage { get; set; }
    }

    public class WeightLimits
    {
        public string Header { get; set; }
        public List<WeightLimitItem> ListWeightLimitItems { get; set; }
    }

    public class WeightLimitItem
    {
        public string Name { get; set; }
        public string LimitsMessage { get; set; }
    }
}
