using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.SeatMap
{

    [Serializable]
    public class MOBPriorityBoarding
    {
        private string productCode = string.Empty;
        private string productName = string.Empty;
        private MOBOfferTile pbOfferTileInfo = null;
        private List<MOBTypeOption> pbOfferDetails = null;
        private List<MOBTypeOption> tAndC = null;
        private List<MOBPBSegment> segments = null; // populate for scenario PB purchased. 
        private string pbDetailsConfirmationTxt = string.Empty; // Priority Boarding details
        private string pbAddedTravelerTxt = string.Empty; // Priority Boarding added for the traveler(s) below

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
        public MOBOfferTile PbOfferTileInfo
        {
            get { return pbOfferTileInfo; }
            set { pbOfferTileInfo = value; }
        }

        public List<MOBTypeOption> PbOfferDetails
        {
            get { return pbOfferDetails; }
            set { pbOfferDetails = value; }
        }

        public List<MOBPBSegment> Segments
        {
            get { return segments; }
            set { segments = value; }
        }

        public List<MOBTypeOption> TAndC
        {
            get { return tAndC; }
            set { tAndC = value; }
        }

        public string PbDetailsConfirmationTxt
        {
            get { return pbDetailsConfirmationTxt; }
            set { pbDetailsConfirmationTxt = value; }
        }

        public string PbAddedTravelerTxt
        {
            get { return pbAddedTravelerTxt; }
            set { pbAddedTravelerTxt = value; }
        }
    }
}
