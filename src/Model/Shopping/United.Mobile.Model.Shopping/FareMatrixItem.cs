using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
   public class FareMatrixItem
    {
       
        public string DepartureDate { get; set; } = string.Empty;
      
        public string ArrivalDate { get; set; } = string.Empty;
      
        public bool IsSelectable { get; set; } 

        //public string Key
        //{
        //    get
        //    {
        //        return this.key;
        //    }
        //    set
        //    {
        //        this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        //    }
        //}

        //public string TripId
        //{
        //    get
        //    {
        //        return this.tripId;
        //    }
        //    set
        //    {
        //        this.tripId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        //    }
        //}

        //public string ProductId
        //{
        //    get
        //    {
        //        return this.productId;
        //    }
        //    set
        //    {
        //        this.productId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        //    }
        //}

        public string DisplayValue { get; set; } = string.Empty;
      

        //public string Value
        //{
        //    get
        //    {
        //        return this.value;
        //    }
        //    set
        //    {
        //        this.value = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        //    }
        //}
    }
}
