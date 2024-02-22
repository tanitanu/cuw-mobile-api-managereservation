using System;
using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    [Serializable]
    public class MOBBundleProduct
    {
        private string productID;
        private string productCode;
        private List<string> productIDs;
        private MOBBundleTile tile;
        private MOBBundleDetail detail;
        private int amount;
        private int productIndex;



        public string ProductID
        {
            get { return productID; }
            set { productID = value; }
        }
        public List<string> ProductIDs
        {
            get { return productIDs; }
            set { productIDs = value; }
        }
        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        public MOBBundleTile Tile
        {
            get { return tile; }
            set { tile = value; }
        }

        public MOBBundleDetail Detail
        {
            get { return detail; }
            set { detail = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public int ProductIndex
        {
            get { return productIndex; }
            set { productIndex = value; }
        }

    }
}
