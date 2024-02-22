using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.SeatMapEngine
{
    [Serializable]
    public class MOBSHOPShoppingProductDetail
    {
        private string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { title = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string header = string.Empty;
        public string Header
        {
            get { return header; }
            set { header = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string body = string.Empty;
        public string Body
        {
            get { return body; }
            set { body = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private List<string> productDetails;
        public List<string> ProductDetails
        {
            get { return productDetails; }
            set { productDetails = value; }
        }


        private string pqdText = string.Empty;
        public string PqdText
        {
            get { return pqdText; }
            set { pqdText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string pqmText = string.Empty;
        public string PqmText
        {
            get { return pqmText; }
            set { pqmText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string rdmText = string.Empty;
        public string RdmText
        {
            get { return rdmText; }
            set { rdmText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private List<MOBSHOPShoppingProductDetailCabinMessage> productCabinMessages;
        public List<MOBSHOPShoppingProductDetailCabinMessage> ProductCabinMessages
        {
            get { return productCabinMessages; }
            set { productCabinMessages = value; }
        }
    }
}
