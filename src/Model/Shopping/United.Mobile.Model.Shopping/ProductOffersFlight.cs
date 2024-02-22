using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ProductOffersFlight : MOBSHOPFlight
    {
        
        //public string SegID
        //{
        //    get
        //    {
        //        return this.segID;
        //    }
        //    set
        //    {
        //        this.segID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        //    }
        //}

        //public int SegmentNumber
        //{
        //    get
        //    {
        //        return this.segNumber;
        //    }
        //    set
        //    {
        //        this.segNumber = value;
        //    }
        //}

        public List<MOBSHOPTraveler> Travelers { get; set; }
       
        public List<ProductOffersPrice> Offers { get; set; }
        public ProductOffersFlight()
        {
            Travelers = new List<MOBSHOPTraveler>();
            Offers = new List<ProductOffersPrice>();
        }
       
    }
}
