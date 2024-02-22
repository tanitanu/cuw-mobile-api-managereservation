using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.UpgradeCabin
{
    [Serializable()]
    public class MOBUpgradeCabinOptionContent
    {
        private string imageUrl;
        private string product;
        private string header;
        private string body;
        private List<string> bodyItems;

        public string ImageUrl { get { return imageUrl; } set { imageUrl = value; } }
        public string Product { get { return product; } set { product = value; } }
        public string Header { get { return header; } set { header = value; } }
        public string Body { get { return body; } set { body = value; } }
        public List<string> BodyItems { get { return bodyItems; } set { bodyItems = value; } }
    }
}
