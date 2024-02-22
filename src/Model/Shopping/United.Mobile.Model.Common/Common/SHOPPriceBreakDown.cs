using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{ 
    [Serializable()]
     public class SHOPPriceBreakDown
     {
        public List<PriceBreakDownRow> PriceBreakDownDetails { get; set; }


        public List<PriceBreakDownRow> PriceBreakDownSummary { get; set; }

        public SHOPPriceBreakDown()
        {
            PriceBreakDownDetails = new List<PriceBreakDownRow>();
            PriceBreakDownSummary = new List<PriceBreakDownRow>();
        }
     }
     [Serializable()]
     public class PriceBreakDownRow
     {
         public string LeftItem { get; set; } = string.Empty;

         public string RightItem { get; set; } = string.Empty;

     }
}
