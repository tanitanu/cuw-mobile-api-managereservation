using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBBasicEconomyBuyOut
    {
        private string productCode;
        private MOBItem elfRestrictionsBeBuyOutLink;
        private List<string> productIds;
        private List<MOBBeBuyOutSegment> productDetail;
        private List<MOBItem> captions;
        private MOBMobileCMSContentMessages termsAndConditions;
        private List<MOBItem> faqs;
        private MOBBeBuyOutOffer offerTile;

        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        public MOBItem ElfRestrictionsBeBuyOutLink
        {
            get { return elfRestrictionsBeBuyOutLink; }
            set { elfRestrictionsBeBuyOutLink = value; }
        }

        public List<string> ProductIds
        {
            get { return productIds; }
            set { productIds = value; }
        }

        public List<MOBItem> Captions
        {
            get { return captions; }
            set { captions = value; }
        }

        public MOBMobileCMSContentMessages MobileCmsContentMessages
        {
            get { return termsAndConditions; }
            set { termsAndConditions = value; }
        }

        public List<MOBItem> Faqs
        {
            get { return faqs; }
            set { faqs = value; }
        }

        public List<MOBBeBuyOutSegment> ProductDetail
        {
            get { return productDetail; }
            set { productDetail = value; }
        }

        public MOBBeBuyOutOffer OfferTile
        {
            get { return offerTile; }
            set { offerTile = value; }
        }
    }
}
