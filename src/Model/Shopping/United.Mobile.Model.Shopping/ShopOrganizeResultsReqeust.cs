using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ShopOrganizeResultsReqeust : MOBRequest
    {
        public string CartId { get; set; } = string.Empty;
        
        public string SessionId { get; set; } = string.Empty;
      
        public MOBSearchFilters SearchFiltersIn { get; set; } 
       
        public int LastTripIndexRequested { get; set; } 
    }
}
