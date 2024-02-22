using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class MOBAcceleratorTraveler
    {
        private string id;
        private string name;
        private string currentMiles;
        private string premierInfo;
        private string mileagePlusNumber;
        private List<MOBAcceleratorOffer> offers;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string CurrentMiles
        {
            get { return currentMiles; }
            set { currentMiles = value; }
        }

        public string PremierMiles
        {
            get { return premierInfo; }
            set { premierInfo = value; }
        }

        public string MileagePlusNumber
        {
            get { return mileagePlusNumber; }
            set { mileagePlusNumber = value; }
        }

        public List<MOBAcceleratorOffer> Offers
        {
            get { return offers; }
            set { offers = value; }
        }

    }
}
