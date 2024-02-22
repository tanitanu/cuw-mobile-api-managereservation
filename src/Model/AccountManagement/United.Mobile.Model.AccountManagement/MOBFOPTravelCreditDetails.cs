using United.Definition.Shopping;
using System;
using System.Collections.Generic;


namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBFOPTravelCreditDetails
    {
		private List<MOBMobileCMSContentMessages> reviewTravelCreditMessages;
        private double totalRedeemAmount;
        public List<MOBMobileCMSContentMessages> ReviewTravelCreditMessages
		{
			get { return reviewTravelCreditMessages; }
			set { reviewTravelCreditMessages = value; }
		}

        private List<MOBFOPTravelCredit> travelCredits;
        public List<MOBFOPTravelCredit> TravelCredits
        {
            get { return travelCredits; }
            set { travelCredits = value; }
        }

        public double TotalRedeemAmount
        {
            get
            {
                totalRedeemAmount = 0;
                if (travelCredits != null && travelCredits.Count > 0)
                {
                    foreach (var travelCredit in travelCredits)
                    {
                        if(travelCredit.IsApplied)
                            totalRedeemAmount += travelCredit.RedeemAmount;
                    }
                }
                return totalRedeemAmount;
            }
        }
    }
}
