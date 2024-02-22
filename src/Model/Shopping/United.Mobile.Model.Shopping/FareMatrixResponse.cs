using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class FareMatrixResponse: MOBResponse
    {
        public List<List<FareMatrixItem>> FareMatrix { get; set; }

        public string CallDurationText {get; set; } = string.Empty;

        public MOBSHOPShopRequest shopRequest { get; set; } 
        public int TravelerCount { get; set; } 
        
        //public List<string> Disclaimer
        //{
        //    get
        //    {
        //        return this.disclaimer;
        //    }
        //    set
        //    {
        //        this.disclaimer = value;
        //    }
        //}
        public string CartId { get; set; } = string.Empty;
        
       
    }
}
