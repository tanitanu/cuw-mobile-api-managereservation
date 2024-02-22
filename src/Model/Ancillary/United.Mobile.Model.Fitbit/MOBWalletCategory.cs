using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Fitbit
{
    [Serializable()]
    public class MOBWalletCategory
    {
        private int categoryId;
        private string categoryName = string.Empty;
        private List<MOBWalletItem> walletItems;

      
        public MOBWalletCategory(int categoryId, string categoryName)
        {
            this.categoryId = categoryId;
            this.categoryName = categoryName;
        }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public List<MOBWalletItem> WalletItems { get; set; }
        public MOBWalletCategory()
        {
            WalletItems = new List<MOBWalletItem>();
        }

    }
}
