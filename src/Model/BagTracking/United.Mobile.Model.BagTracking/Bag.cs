using System;
using System.Collections.Generic;

namespace United.Mobile.Model.BagTracking
{
    [Serializable]
    public class Bag
    {
        public List<BagHistory> BagHistories { get; set; }
        public BagItinerary BagItinerary { get; set; }
        public BagTag BagTag { get; set; }
        public Passenger Passenger { get; set; }
        public bool BagRerouted { get; set; }
        public Bag()
        {
            BagHistories = new List<BagHistory>();
        }

    }
}
