using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBBKShoppingProduct
    {
        private string productId = string.Empty;
        public string ProductId
        {
            get { return productId; }
            set { productId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string type = string.Empty;
        public string Type
        {
            get { return type; }
            set { type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string cabin = string.Empty;
        public string Cabin
        {
            get { return cabin; }
            set { cabin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string description = string.Empty;
        public string Description
        {
            get { return description; }
            set { description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string price = string.Empty;
        public string Price
        {
            get { return price; }
            set { price = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

    }
}
