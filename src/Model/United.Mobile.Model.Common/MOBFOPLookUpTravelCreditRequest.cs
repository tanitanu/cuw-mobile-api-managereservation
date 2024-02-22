using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBFOPLookUpTravelCreditRequest : MOBRequest
    {
        public string ObjectName { get; set; } = "United.Definition.MOBFOPLookUpTravelCreditRequest";
        private string lastName;
        private string pinOrPnr;
        private string hashPin = string.Empty;
        private string mileagePlusNumber = string.Empty;


        public string LastName { get { return lastName; } set { lastName = value; } }
        public string PinOrPnr { get { return pinOrPnr; } set { pinOrPnr = value; } }
        public string MileagePlusNumber { get { return mileagePlusNumber; } set { mileagePlusNumber = value; } }
        public string HashPin { get { return hashPin; } set { hashPin = value; } }
    }
}
