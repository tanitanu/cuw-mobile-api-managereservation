using System;
using System.Collections.Generic;
using United.Definition.Shopping;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class Person
    {
        public string Key { get; set; } = string.Empty;

        public MOBSHOPCountry CountryOfResidence { get; set; }

        public string DateOfBirth { get; set; } = string.Empty;

        public string GivenName { get; set; } = string.Empty;

        public string MiddleName { get; set; } = string.Empty;

        public MOBSHOPCountry Nationality { get; set; }

        public string PreferredName { get; set; } = string.Empty;

        public string Sex { get; set; } = string.Empty;

        public string Suffix { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public List<Document> Documents { get; set; }
        public MOBAddress Address { get; set; }

        public List<Phone> Phones { get; set; }
        public string RedressNumber { get; set; } = string.Empty;

        public string KnownTravelerNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

    }
}
