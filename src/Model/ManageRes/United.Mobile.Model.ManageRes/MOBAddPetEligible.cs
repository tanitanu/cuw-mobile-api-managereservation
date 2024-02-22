using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.ManageRes
{
    public class MOBAddPetEligible
    {
        private string addPetRedirectURL;
        private bool addPetEligible;
        private string addPetButtonText;


        public string AddPetRedirectURL
        {
            get { return addPetRedirectURL; }
            set { addPetRedirectURL = value; }
        }
        public bool AddPetEligible
        {
            get { return addPetEligible; }
            set { addPetEligible = value; }
        }
        public string AddPetButtonText
        {
            get { return addPetButtonText; }
            set { addPetButtonText = value; }
        }
    }
}
