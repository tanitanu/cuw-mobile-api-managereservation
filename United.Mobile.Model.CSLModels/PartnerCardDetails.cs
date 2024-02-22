using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class PartnerCardDetails
    {
       
            public bool? ExpMilesWaiver
            {
                get;
                set;
            }


            public bool? MinSegmentWaiver
            {
                get;
                set;
            }


            public long CustomerId
            {
                get;
                set;
            }


            public string LoyaltyId
            {
                get;
                set;
            }


            public string TravelBankAccountNumer
            {
                get;
                set;
            }


            public List<PartnerCard> PartnerCards
            {
                get;
                set;
            }

        
    }
    public class PartnerCard
    {

        public string PartnerCode
        {
            get;
            set;
        }


        public string CardType
        {
            get;
            set;
        }


        public string CardTypeDescription
        {
            get;
            set;
        }


        public string ProductType
        {
            get;
            set;
        }


        public string ARN
        {
            get;
            set;
        }


        public DateTime? AFBillingDate
        {
            get;
            set;
        }


        public string AccountCode
        {
            get;
            set;
        }


        public string Key
        {
            get;
            set;
        }


        public int Hierarchy
        {
            get;
            set;
        }
    }
}
