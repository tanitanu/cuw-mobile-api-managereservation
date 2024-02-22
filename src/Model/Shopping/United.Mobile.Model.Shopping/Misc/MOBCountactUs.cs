using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using United.Mobile.Model.Common;
//using United.Mobile.Model.Shopping.Booking;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class ContactUsResponse : MOBResponse
    {
        private ContactUsUSACanada contactUsUSACanada;
        public ContactUsUSACanada ContactUsUSACanada
        {
            get
            {
                return this.contactUsUSACanada;
            }
            set
            {
                this.contactUsUSACanada = value;
            }
        }


        //private InternationalCountryListInfo obj1;
        private ContactUSOutSideUSACanada contactUSOutSideUSACanada;

        public ContactUSOutSideUSACanada ContactUSOutSideUSACanada
        {
            get { return contactUSOutSideUSACanada; }
            set { contactUSOutSideUSACanada = value; }
        }

    }
    [Serializable()]
    public class ContactUsUSACanada
    {
        private string domesticContactTabHeaderText = System.Configuration.ConfigurationManager.AppSettings["DomesticContactUSTabHeaderText"];// U.S./Canada

        public string DomesticContactTabHeaderText
        {
            get { return domesticContactTabHeaderText; }
            set { domesticContactTabHeaderText = System.Configuration.ConfigurationManager.AppSettings["DomesticContactUSTabHeaderText"]; }
        }

        private ContactUSUSACanadaContactTypePhone usaCanadaContactTypePhone;
        public ContactUSUSACanadaContactTypePhone USACanadaContactTypePhone
        {
            get { return usaCanadaContactTypePhone; }
            set { usaCanadaContactTypePhone = value; }
        }

        private ContactUSContactTypeEmail usaCanadaContactTypeEmail;
        public ContactUSContactTypeEmail USACanadaContactTypeEmail
        {
            get { return usaCanadaContactTypeEmail; }
            set { usaCanadaContactTypeEmail = value; }
        }

    }
    [Serializable]
    public class ContactUSUSACanadaContactTypePhone
    {
        private string contactType;

        public string ContactType
        {
            get { return contactType; }
            set { contactType = value; }
        }

        private List<ContactUSUSACanadaPhoneNumber> phoneNumbers;
        public List<ContactUSUSACanadaPhoneNumber> PhoneNumbers
        {
            get
            {
                return this.phoneNumbers;
            }
            set
            {
                this.phoneNumbers = value;
            }
        }
    }
    [Serializable]
    public class ContactUSContactTypeEmail
    {
        private string contactType;

        public string ContactType
        {
            get { return contactType; }
            set { contactType = value; }
        }

        private List<ContactUSEmail> emailAddresses;
        public List<ContactUSEmail> EmailAddresses
        {
            get
            {
                return this.emailAddresses;
            }
            set
            {
                this.emailAddresses = value;
            }
        }

    }
    [Serializable]
    public class ContactUSUSACanadaPhoneNumber
    {
        private string contactUsDeskName = string.Empty;
        private string contactUsDeskDescription = string.Empty;
        private string contactUsDeskPhoneNumber = string.Empty;
        public string ContactUsDeskName
        {
            get
            {
                return this.contactUsDeskName;
            }
            set
            {
                this.contactUsDeskName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ContactUsDeskDescription
        {
            get
            {
                return this.contactUsDeskDescription;
            }
            set
            {
                this.contactUsDeskDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ContactUsDeskPhoneNumber
        {
            get
            {
                return this.contactUsDeskPhoneNumber;
            }
            set
            {
                this.contactUsDeskPhoneNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
    [Serializable]
    public class ContactUSEmail
    {
        private string contactUsDeskEmailName = string.Empty;
        private string contactUsDeskEmailAdress = string.Empty;
        public string ContactUsDeskEmailName
        {
            get
            {
                return this.contactUsDeskEmailName;
            }
            set
            {
                this.contactUsDeskEmailName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string ContactUsDeskEmailAddress
        {
            get
            {
                return this.contactUsDeskEmailAdress;
            }
            set
            {
                this.contactUsDeskEmailAdress = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
    [Serializable()]
    public class ContactUSOutSideUSACanada
    {
        private string internationalTabHeaderText = System.Configuration.ConfigurationManager.AppSettings["InternationalContactUsTabHeaderText"]; // Outside U.S./Canada

        public string InternationalTabHeaderText
        {
            get { return internationalTabHeaderText; }
            set { internationalTabHeaderText = System.Configuration.ConfigurationManager.AppSettings["InternationalContactUsTabHeaderText"]; }
        }

        private string defaultEmailAddressContactType;
        public string DefaultEmailAddressContactType
        {
            get { return defaultEmailAddressContactType; }
            set { defaultEmailAddressContactType = value; }
        }

        private string internaitonPhoneContactType;
        public string InternaitonPhoneContactType
        {
            get { return internaitonPhoneContactType; }
            set { internaitonPhoneContactType = value; }
        }

        private string selectCountryDefaultText;/// <summary>
                                                /// Select a country to find an AT&T Direct Access Number.
                                                /// </summary>
        public string SelectCountryDefaultText
        {
            get { return selectCountryDefaultText; }
            set { selectCountryDefaultText = value; }
        }

        private string selectCountryFromListScreenText; /// <summary>
                                                        /// For countries not listed , visit business.att.com/....
                                                        /// </summary>
        public string SelectCountryFromListScreenText
        {
            get { return selectCountryFromListScreenText; }
            set { selectCountryFromListScreenText = value; }
        }

        private string countryListDefaultSelection; 
        public string CountryListDefaultSelection
        {
            get { return countryListDefaultSelection; }
            set { countryListDefaultSelection = value; }
        }

        private List<ContactUSEmail> internationalDefaultEmailAddresses;
        public List<ContactUSEmail> InternationalDefaultEmailAddresses
        {
            get
            {
                return this.internationalDefaultEmailAddresses;
            }
            set
            {
                this.internationalDefaultEmailAddresses = value;
            }
        }

        /// <summary>
        /// Dial the AT&T Direct Access Number for your city
        /// </summary>
        private string aTTAccessNumberDialInfoText;
        public string ATTAccessNumberDialInfoText
        {
            get { return aTTAccessNumberDialInfoText; }
            set { aTTAccessNumberDialInfoText = value; }
        }
        /// <summary>
        /// Once you dial the AT&T Direct Access Number, an English language voice prompt or an AT&T operator will ask you to enter this toll-free number
        /// </summary>
        private string howToUseOutsideUSACanadaATTDirectAccessNumberDescription = string.Empty;
        public string HowToUseOutsideUSACanadaATTDirectAccessNumberDescription
        {
            get
            {
                return this.howToUseOutsideUSACanadaATTDirectAccessNumberDescription;
            }
            set
            {
                this.howToUseOutsideUSACanadaATTDirectAccessNumberDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string outSideUSACanadaContactATTTollFreeNumber = string.Empty;
        public string OutSideUSACanadaContactATTTollFreeNumber
        {
            get
            {
                return this.outSideUSACanadaContactATTTollFreeNumber;
            }
            set
            {
                this.outSideUSACanadaContactATTTollFreeNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }


        private List<ContactUSOusideUSACanadaContactTypePhone> outSideUSACanadaContactTypePhoneList;
        public List<ContactUSOusideUSACanadaContactTypePhone> OutSideUSACanadaContactTypePhoneList
        {
            get { return outSideUSACanadaContactTypePhoneList; }
            set { outSideUSACanadaContactTypePhoneList = value; }
        }

        private string contactUSLocationDescription;
        public string ContactUSLocationDescription
        {
            get { return contactUSLocationDescription; }
            set { contactUSLocationDescription = value; }
        }
        private string contactUSLocationHyperlink;
        public string ContactUSLocationHyperlink
        {
            get { return contactUSLocationHyperlink; }
            set { contactUSLocationHyperlink = value; }
        }
        private string contactUSDirectAccessNumber;
        public string ContactUSDirectAccessNumber
        {
            get { return contactUSDirectAccessNumber; }
            set { contactUSDirectAccessNumber = value; }
        }

    }
    [Serializable]
    public class ContactUSOusideUSACanadaContactTypePhone
    {
        private BKCountry country;
        public BKCountry Country
        {
            get { return country; }
            set { country = value; }
        }

        private List<ContactAccessNumber> contactAccessNumberList = null;
        public List<ContactAccessNumber> ContactAccessNumberList
        {
            get
            {
                return this.contactAccessNumberList;
            }
            set
            {
                this.contactAccessNumberList = value;
            }
        }
        // Define the internation phone number model. 
    }
    [Serializable()]
    public class ContactAccessNumber
    {
        private string city = string.Empty;
        private List<string> attDirectAccessNumbers;

        public string City
        {
            get
            {
                return this.city;
            }
            set
            {
                this.city = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<string> ATTDirectAccessNumbers
        {
            get
            {
                return this.attDirectAccessNumbers;
            }
            set
            {
                this.attDirectAccessNumbers = value;
            }
        }
    }
    [Serializable]
    public class ContactUsRequest : MOBRequest
    {
        private MemberType memberType = MemberType.GENERAL;
        private string mileagePlusNumber = string.Empty;
        private string hashValue;
        private Boolean isCEO;
        public MemberType MemberType
        {
            get
            {
                return this.memberType;
            }
            set
            {
                this.memberType = value;
            }
        }
        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
        public string HashValue
        {
            get { return hashValue; }
            set { hashValue = value; }
        }
        public Boolean IsCEO
        {
            get
            {
                return this.isCEO;
            }
            set
            {
                this.isCEO = value;
            }
        }

    }
    [Serializable()]
    public enum MemberType
    {
        [EnumMember(Value = "0")]
        GENERAL,
        [EnumMember(Value = "1")]
        PremierSilver,
        [EnumMember(Value = "2")]
        PremierGold,
        [EnumMember(Value = "3")]
        PremierPlatinium,
        [EnumMember(Value = "4")]
        Premier1K,
        [EnumMember(Value = "5")]
        PremierGlobalServices,
        [EnumMember(Value = "-1")]
        CHairManCircle
    }
}
