using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    public class PriorityBoardingFile //: PersistBase, IPersist
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.PriorityBoarding.PriorityBoardingFile";

        public string ObjectName
        {
            get { return objectName; }
            set { objectName = value; }
        }
        #endregion

        private PriorityBoarding priorityBoarding;

        public PriorityBoarding PriorityBoarding
        {
            get { return priorityBoarding; }
            set { priorityBoarding = value; }
        }

        private MOBPriorityBoardingSelectionResponse pBSelectionResponse;

        public MOBPriorityBoardingSelectionResponse PBSelectionResponse
        {
            get { return pBSelectionResponse; }
            set { pBSelectionResponse = value; }
        }

        private string pBSelectionTags;

        public string PBSelectionTags
        {
            get { return pBSelectionTags; }
            set { pBSelectionTags = value; }
        }

        private string offerResponse;

        public string OfferResponse
        {
            get { return offerResponse; }
            set { offerResponse = value; }
        }

        private string recordLocator;

        public string RecordLocator
        {
            get { return recordLocator; }
            set { recordLocator = value; }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private string pNRCreationDate;
        public string PNRCreationDate
        {
            get { return pNRCreationDate; }
            set { pNRCreationDate = value; }
        }

        private int numberOfTraveler;
        public int NumberOfTraveler
        {
            get { return numberOfTraveler; }
            set { numberOfTraveler = value; }
        }

        private string cartId;
        public string CartID
        {
            get { return cartId; }
            set { cartId = value; }
        }

        private string langCode;
        public string LangCode
        {
            get { return langCode; }
            set { langCode = value; }
        }

        private string countryCode;
        public string CountryCode
        {
            get { return countryCode; }
            set { countryCode = value; }
        }
    }
}
