using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.PromoCode
{
   
        [Serializable]
        public class MOBTPIInfo
        {
            private string productCode = string.Empty;
            private string productName = string.Empty;
            private string displayAmount = string.Empty; // TPI price $14.12
            private string formattedDisplayAmount = string.Empty; // $15, removed 
            private double amount; // 14.12
            private string coverCost = string.Empty; // TPI cover price $566.40
            private string pageTitle = string.Empty; // Travel Guard® Insurance 
            private string title1 = string.Empty; // Cover your trip with a 
            private string title2 = string.Empty; // For
            private string title3 = string.Empty; // per person, removed 
            private string quoteTitle = string.Empty; // Travel Guard® Insurance by AIG Travel
            private string headline1 = string.Empty; // Travel Guard Insurance from AIG 
            private string headline2 = string.Empty; // Add Travel Guard Insurance 
            private string body1 = string.Empty; // Are you prepard?
            private string body2 = string.Empty; // From millions of travelers every year...
            private string body3 = string.Empty; // Coverage is covered by...
            private string lineItemText = string.Empty; // Covers total trip cost 
            private string tNC = string.Empty; // By clicking on purchase...
            private string image = string.Empty;
            private string productId = string.Empty; // QuoteId 
            private string paymentContent = string.Empty; // Travel Guard® Insurance by AIG Travel
            private string pkDispenserPublicKey = string.Empty;
            private string confirmation1 = string.Empty;
            private string confirmation2 = string.Empty;
            private List<MOBItem> tpiAIGReturnedMessageContentList;
            private string tileImage = string.Empty;
            private string tileTitle1 = string.Empty;
            private string tileTitle2 = string.Empty;
            private string tileQuoteTitle = string.Empty;
            public List<MOBItem> TPIAIGReturnedMessageContentList
            {
                get
                {
                    return this.tpiAIGReturnedMessageContentList;
                }
                set
                {
                    this.tpiAIGReturnedMessageContentList = value;
                }
            }

            public string ProductCode
            {
                get { return productCode; }
                set { productCode = value; }
            }
            public string ProductName
            {
                get { return productName; }
                set { productName = value; }
            }
            public string DisplayAmount
            {
                get
                {
                    return this.displayAmount;
                }
                set
                {
                    this.displayAmount = string.IsNullOrEmpty(value) ? string.Empty : value;
                }
            }
            public string FormattedDisplayAmount
            {
                get
                {
                    return this.formattedDisplayAmount;
                }
                set
                {
                    this.formattedDisplayAmount = string.IsNullOrEmpty(value) ? string.Empty : value;
                }
            }
            public double Amount
            {
                get
                {
                    return this.amount;
                }
                set
                {
                    this.amount = value;
                }
            }
            public string CoverCost
            {
                get
                {
                    return this.coverCost;
                }
                set
                {
                    this.coverCost = string.IsNullOrEmpty(value) ? string.Empty : value;
                }
            }
            public string PageTitle
            {
                get
                {
                    return this.pageTitle;
                }
                set
                {
                    this.pageTitle = string.IsNullOrEmpty(value) ? string.Empty : value;
                }
            }
            public string Title1
            {
                get
                {
                    return this.title1;
                }
                set
                {
                    this.title1 = string.IsNullOrEmpty(value) ? string.Empty : value;
                }
            }
            public string Title2
            {
                get
                {
                    return this.title2;
                }
                set
                {
                    this.title2 = string.IsNullOrEmpty(value) ? string.Empty : value;
                }
            }

            public string Title3
            {
                get
                {
                    return this.title3;
                }
                set
                {
                    this.title3 = string.IsNullOrEmpty(value) ? string.Empty : value;
                }
            }

            public string QuoteTitle
            {
                get
                {
                    return this.quoteTitle;
                }
                set
                {
                    this.quoteTitle = string.IsNullOrEmpty(value) ? string.Empty : value;
                }
            }
            public string Headline1
            {
                get
                {
                    return this.headline1;
                }
                set
                {
                    this.headline1 = string.IsNullOrEmpty(value) ? string.Empty : value;
                }
            }
            public string Headline2
            {
                get
                {
                    return this.headline2;
                }
                set
                {
                    this.headline2 = string.IsNullOrEmpty(value) ? string.Empty : value;
                }
            }
            public string Body1
            {
                get
                {
                    return this.body1;
                }
                set
                {
                    this.body1 = string.IsNullOrEmpty(value) ? string.Empty : value;
                }
            }
            public string Body2
            {
                get
                {
                    return this.body2;
                }
                set
                {
                    this.body2 = string.IsNullOrEmpty(value) ? string.Empty : value;
                }
            }
            public string Body3
            {
                get
                {
                    return this.body3;
                }
                set
                {
                    this.body3 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                }
            }
            public string LineItemText
            {
                get
                {
                    return this.lineItemText;
                }
                set
                {
                    this.lineItemText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                }
            }
            public string TNC
            {
                get
                {
                    return this.tNC;
                }
                set
                {
                    this.tNC = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                }
            }

            public string Image
            {
                get
                {
                    return this.image;
                }
                set
                {
                    this.image = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                }
            }

            public string ProductId
            {
                get
                {
                    return this.productId;
                }
                set
                {
                    this.productId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                }
            }

            public string PaymentContent
            {
                get
                {
                    return this.paymentContent;
                }
                set
                {
                    this.paymentContent = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                }
            }
            public string PkDispenserPublicKey
            {
                get
                {
                    return this.pkDispenserPublicKey;
                }
                set
                {
                    this.pkDispenserPublicKey = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                }
            }
            public string Confirmation1
            {
                get
                {
                    return this.confirmation1;
                }
                set
                {
                    this.confirmation1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                }
            }
            public string Confirmation2
            {
                get
                {
                    return this.confirmation2;
                }
                set
                {
                    this.confirmation2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                }
            }

           public string TileImage
           {
               get
               {
                   return this.tileImage;
               }
               set
               {
                   this.tileImage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
               }
           }
        public string TileTitle1
        {
            get
            {
                return this.tileTitle1;
            }
            set
            {
                this.tileTitle1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string TileTitle2
        {
            get
            {
                return this.tileTitle2;
            }
            set
            {
                this.tileTitle2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string TileQuoteTitle
        {
            get
            {
                return this.tileQuoteTitle;
            }
            set
            {
                this.tileQuoteTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

    }
    
}
