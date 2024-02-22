using System.Collections.Generic;

namespace United.Mobile.Model.Internal.CompleteBooking
{
    public class UnaccompaniedMinorAdultContactPerson
    {
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public List<UnaccompaniedMinorTripDropOffPickUpInfo> DropOffPickUpProfile { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PhoneNumberCountryCode { get; set; } = string.Empty;
        public string PhoneNumberExtensionOrPin { get; set; } = string.Empty;
        public string PhoneNumberType { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public string StateOrProvince { get; set; } = string.Empty;
        public string StreetAddressLineOne { get; set; } = string.Empty;
        public string StreetAddressLineThree { get; set; } = string.Empty;
        public string StreetAddressLineTwo { get; set; } = string.Empty;
        public string ZipOrPostalCode { get; set; } = string.Empty;
        public string AdultContactStatus { get; set; }
    }
}