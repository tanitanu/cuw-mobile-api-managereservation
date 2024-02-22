using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public sealed class RewardType
    {
        private int _key;
        private string _type;
        private string _description;
        private string _productID;

        public RewardType() { }

        internal RewardType(int key, string type, string description, string productID)
        {
            _key = key;
            _type = type;
            _description = description;
            _productID = productID;
        }

        public int Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public string ProductID
        {
            get
            {
                return _productID;
            }
            set
            {
                _productID = value;
            }
        }
    }
}
