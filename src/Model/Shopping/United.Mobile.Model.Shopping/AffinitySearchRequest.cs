using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class AffinitySearchRequest: MOBRequest
    {
        public string Origin { get; set; } = string.Empty;
       
        public string Destination { get; set; } = string.Empty;
        
        public string PointOfSale { get; set; } = string.Empty;
       
        public string MinDepartDate { get; set; } = string.Empty;
       
        public string MaxDepartDate { get; set; } = string.Empty;
        
        public string MinReturnDate { get; set; } = string.Empty;
       
        public string MaxReturnDate { get; set; } = string.Empty;
       
        public int TripDurationDays { get; set; } 
        
        public bool IsOneWay { get; set; }
       
    }
}
