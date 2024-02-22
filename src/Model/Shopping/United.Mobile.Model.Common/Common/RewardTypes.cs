using System.Collections.Specialized;

namespace United.Mobile.Model.Shopping
{
    public sealed class RewardTypes
    {
        private HybridDictionary _rewardTypes;

        /// <summary>
        /// Default constructor
        /// </summary>
        public RewardTypes()
        {
            _rewardTypes = new HybridDictionary();
        }

        /// <summary>
        /// Index accessor to hybrid dictionary
        /// </summary>
        public RewardType this[int Key]
        {
            get
            {
                return _rewardTypes[Key] as RewardType;
            }
        }

        /// <summary>
        /// Count of hybird dictionary
        /// </summary>
        public int Count
        {
            get
            {
                return _rewardTypes.Count;
            }
        }

        internal void Add(RewardType rewardType)
        {
            _rewardTypes.Add(rewardType.Key, rewardType);
        }


        internal void Clear()
        {
            _rewardTypes.Clear();
        }
    }
}
