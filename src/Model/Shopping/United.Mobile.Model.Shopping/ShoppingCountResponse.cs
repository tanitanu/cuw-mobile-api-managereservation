using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ShoppingCountResponse :MOBResponse
    {
        private ShoppingCountRequest shoppingCountRequest;
        private string shoppingCountMessage;

        public ShoppingCountRequest ShoppingCountRequest
        {
            get
            {
                return this.shoppingCountRequest;
            }
            set
            {
                this.shoppingCountRequest = value;
            }
        }

        public string ShoppingCountMessage
        {
            get
            {
                return this.shoppingCountMessage;
            }
            set
            {
                this.shoppingCountMessage = value;
            }
        }
    }
}
