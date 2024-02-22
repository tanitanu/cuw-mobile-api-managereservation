using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Payment
{
    [Serializable()]
    public class MOBRegisterOfferRequest : MOBRequest
    {
        private Collection<MerchandizingOfferDetails> merchandizingOfferDetails = null;
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private string flow = string.Empty;

        public Collection<MerchandizingOfferDetails> MerchandizingOfferDetails
        {
            get
            {
                return this.merchandizingOfferDetails;
            }
            set
            {
                this.merchandizingOfferDetails = value;
            }
        }

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = value;
            }
        }
        public string CartId
        {
            get
            {
                return this.cartId;
            }
            set
            {
                this.cartId = value;
            }
        }

        public string Flow
        {
            get
            {
                return this.flow;
            }
            set
            {
                this.flow = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
    public class MerchandizingOfferDetails
    {
        private string productCode = string.Empty;
        private List<string> productIds = new List<string>();
        private bool isOfferRegistered;
        private string subProductCode = string.Empty;
        private List<string> tripIds;
        List<string> selectedTripProductIDs;
        private bool isReQuote = false;
        private bool isAcceptChanges = false;
        private int numberOfPasses;
        public string ProductCode
        {
            get
            {
                return this.productCode;
            }
            set
            {
                this.productCode = value;
            }
        }

        public List<string> ProductIds
        {
            get
            {
                return this.productIds;
            }
            set
            {
                this.productIds = value;
            }
        }
        public bool IsOfferRegistered
        {
            get
            {
                return this.isOfferRegistered;
            }
            set
            {
                this.isOfferRegistered = value;
            }
        }

        public string SubProductCode
        {
            get
            {
                return this.subProductCode;
            }
            set
            {
                this.subProductCode = value;
            }
        }
        public List<string> TripIds
        {
            get
            {
                return this.tripIds;
            }
            set
            {
                this.tripIds = value;
            }
        }

        public List<string> SelectedTripProductIDs
        {
            get
            {
                return this.selectedTripProductIDs;
            }
            set
            {
                this.selectedTripProductIDs = value;
            }
        }
        public bool IsReQuote
        {
            get
            {
                return this.isReQuote;
            }
            set
            {
                this.isReQuote = value;
            }
        }
        public bool IsAcceptChanges
        {
            get
            {
                return this.isAcceptChanges;
            }
            set
            {
                this.isAcceptChanges = value;
            }
        }
        public int NumberOfPasses
        {
            get
            {
                return this.numberOfPasses;
            }
            set
            {
                this.numberOfPasses = value;
            }
        }
    }
}
