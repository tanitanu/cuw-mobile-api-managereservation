using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MetaSelectTripRequest : MOBRequest
    {
        private string medaSessionId = string.Empty;
        private string cartId = string.Empty;
        private bool requeryForUpsell = false;
        private string bbxSolutionId = string.Empty;
        private string bbxCellId = string.Empty;
        private string mileagePlusAccountNumber;
        private int premierStatusLevel;
        private bool isCatalogOnForTavelerTypes;
        private int customerId;

        private string sharedTripId;
        private string source;
        private string platform;
        private string deepLinkType;
        private string typeOfDeeplink;
        private List<MOBItem> catalogItems;

        public string TypeOfDeeplink
        {
            get { return typeOfDeeplink; }
            set { typeOfDeeplink = value; }
        }


        public string DeepLinkType
        {
            get { return deepLinkType; }
            set { deepLinkType = value; }
        }


        public string Platform
        {
            get { return platform; }
            set { platform = value; }
        }


        public string Source
        {
            get { return source; }
            set { source = value; }
        }


        public string SharedTripId
        {
            get { return sharedTripId; }
            set { sharedTripId = value; }
        }

        public int CustomerId
        {
            get
            {
                return this.customerId;
            }
            set
            {
                this.customerId = value;
            }
        }

        public bool IsCatalogOnForTavelerTypes
        {
            get { return isCatalogOnForTavelerTypes; }
            set { isCatalogOnForTavelerTypes = value; }
        }

        public string MedaSessionId
        {
            get
            {
                return this.medaSessionId;
            }
            set
            {
                this.medaSessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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
                this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool RequeryForUpsell
        {
            get { return requeryForUpsell; }
            set { requeryForUpsell = value; }
        }

        public string BbxSolutionId
        {
            get { return bbxSolutionId; }
            set { bbxSolutionId = value; }
        }

        public string BbxCellId
        {
            get { return bbxCellId; }
            set { bbxCellId = value; }
        }

        public string MileagePlusAccountNumber
        {
            get { return mileagePlusAccountNumber; }
            set { mileagePlusAccountNumber = value; }
        }

        public int PremierStatusLevel
        {
            get { return premierStatusLevel; }
            set { premierStatusLevel = value; }
        }

        public string SessionId { get; set; }
        public List<MOBItem> CatalogItems
        {
            get { return catalogItems; }
            set { catalogItems = value; }
        }
    }
}
