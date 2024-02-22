using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class OfferPriceAssociation
    {
        private List<string> travelerRefIDs;
        private List<string> segmentRefIDs;
        private List<OfferODMapping> oDMappings;

        public List<string> TravelerRefIDs { get; set; } 
        public List<string> SegmentRefIDs { get; set; } 
        public List<OfferODMapping> ODMappings { get; set; }
        public OfferPriceAssociation()
        {
            TravelerRefIDs = new List<string>();
            SegmentRefIDs = new List<string>();
            ODMappings = new List<OfferODMapping>();
        }
    }
}
