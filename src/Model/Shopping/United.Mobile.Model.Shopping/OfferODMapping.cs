using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    public class OfferODMapping
    {
        private string refID = string.Empty;
        private List<string> segmentRefIDs;

        public string RefID
        {
            get
            {
                return this.refID;
            }
            set
            {
                this.refID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<string> SegmentRefIDs
        {
            get
            {
                return this.segmentRefIDs;
            }
            set
            {
                this.segmentRefIDs = value;
            }
        }

        //public string RefID { get; set; } = string.Empty;

        //public List<string> SegmentRefIDs { get; set; }
        //public OfferODMapping()
        //{
        //    SegmentRefIDs = new List<string>();
        //}
    }
}
