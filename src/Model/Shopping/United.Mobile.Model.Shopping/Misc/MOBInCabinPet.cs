using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class InCabinPet
    {
        private List<MOBItem> messages;
        public List<MOBItem> Messages { get { return this.messages; } set { this.messages = value; } }
        private string inCabinPetLabel;
        public string InCabinPetLabel
        {
            get { return this.inCabinPetLabel; }
            set { this.inCabinPetLabel = value; }
        }
        private string inCabinPetRefText;
        public string InCabinPetRefText
        {
            get { return this.inCabinPetRefText; }
            set { this.inCabinPetRefText = value; }
        }
        private string inCabinPetRefValue;
        public string InCabinPetRefValue
        {
            get { return this.inCabinPetRefValue; }
            set { this.inCabinPetRefValue = value; }
        }
    }
}
