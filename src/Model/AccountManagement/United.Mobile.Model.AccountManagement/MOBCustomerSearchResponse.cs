using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBCustomerSearchResponse
    {
        public List<MOBCustomerSearchDetail> CustomerSearchDetails { get; set; }
        public List<object> Errors { get; set; }
        public int Status { get; set; }
        public string Source { get; set; }
        public string ServerName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    [Serializable]
    public class MOBCustomerSearchDetail
    {
        public string Title { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public DateTime? EnrollDate { get; set; }
        public string LoyaltyId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string AreaNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string CustomerId { get; set; }
    }
}