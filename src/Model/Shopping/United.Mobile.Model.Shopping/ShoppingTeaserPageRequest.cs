using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ShoppingTeaserPageRequest  : MOBRequest
    {
        public string SessionID { get; set; } = string.Empty;

        public string CartID { get; set; } = string.Empty;

    }
}

