using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.MPRewards
{
    [Serializable()]
    public class MOBGetActionDetailsForOffersRequest : MOBRequest
    {
        private string data;
        private string mileagePlusNumber;
        private List<MOBItem> catalogValues;
        private MOBTravelerSignInData travelerSignInData = null;

        public string MileagePlusNumber
        {
            get { return mileagePlusNumber; }
            set { mileagePlusNumber = value; }
        }
        
        public string Data
        {
            get { return data; }
            set { data = value; }
        }
        public List<MOBItem> CatalogValues
        {
            get
            {
                return this.catalogValues;
            }
            set
            {
                this.catalogValues = value;
            }
        }
        public MOBTravelerSignInData TravelerSignInData
        {
            get { return travelerSignInData; }
            set { travelerSignInData = value; }
        }
    }
}
