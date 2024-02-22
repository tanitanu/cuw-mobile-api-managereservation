using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using United.Definition;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBResTraveler
    {
        private string key = string.Empty;
        private MOBComLoyaltyProgramProfile loyaltyProgramProfile; 
        private MOBPerPerson person;

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public MOBComLoyaltyProgramProfile LoyaltyProgramProfile
        {
            get
            {
                return this.loyaltyProgramProfile;
            }
            set
            {
                this.loyaltyProgramProfile = value;
            }
        }

        public MOBPerPerson Person
        {
            get
            {
                return this.person;
            }
            set
            {
                this.person = value;
            }
        }
    }
}
