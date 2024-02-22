using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Pcu
{
    [Serializable()]
    public class PcuUpgradeOptionInfo
    {
        private string imageUrl;
        private string product;
        private string header;
        private string body;

        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        public string Product
        {
            get { return product; }
            set { product = value; }
        }

        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        public string Body
        {
            get { return body; }
            set { body = value; }
        }
    }
}
