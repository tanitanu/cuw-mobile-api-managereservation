using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class PrefContact
    {

        public long CustomerId { get; set; }

        public long ProfileOwnerId { get; set; }
        public long ContactId { get; set; }

        public int ContactSequenceNum { get; set; }

        public string Key { get; set; } 


        public string ContactTypeCode { get; set; } 


        public string ContactTypeDescription { get; set; } 

        public string Title { get; set; } 

        public string FirstName { get; set; } 


        public string MiddleName { get; set; } 

        public string LastName { get; set; } 

        public string Suffix { get; set; } 

        public string GenderCode { get; set; } 

        public string ContactMileagePlusId { get; set; } 

        public string LanguageCode { get; set; } 

        public bool IsSelected { get; set; }

        public bool IsNew { get; set; }
        public bool IsVictim { get; set; }
        public bool IsDeceased { get; set; }

        public List<PrefPhone> Phones { get; set; }
        public PrefContact()
        {
            Phones = new List<PrefPhone>();
        }
    }
}
