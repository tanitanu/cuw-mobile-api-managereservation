using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Bundles
{
    [Serializable]
   public class BookingBundlesRequest : MOBRequest
    {
        public string CartId { get; set; } = string.Empty;

        public string SessionId { get; set; } = string.Empty;

        public string MPNumber { get; set; } = string.Empty;

        public string CustomerID { get; set; } = string.Empty;

        public string HashCode { get; set; } = string.Empty;

        public string ScreenSize { get; set; } = string.Empty;

        public string Flow { get; set; } = string.Empty;
        private bool isBackNavigationFromRTI;
        public bool IsBackNavigationFromRTI
        {
            get { return isBackNavigationFromRTI; }
            set { isBackNavigationFromRTI = value; }
        }


    }
}
