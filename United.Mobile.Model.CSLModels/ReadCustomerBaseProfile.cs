using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class ReadCustomerBaseProfile:Base
    {
       
            //
            // Summary:
            //     Key - combination of Customer Id and Profile Id


            public string TravelerKey
            {
                get;
                set;
            }

            //
            // Summary:
            //     Document Id


            public object _id
            {
                get;
                set;
            }

            //
            // Summary:
            //     CustomerId required



            public long CustomerId
            {
                get;
                set;
            }

            //
            // Summary:
            //     LoyaltyId required




            public string LoyaltyId
            {
                get;
                set;
            }

            //
            // Summary:
            //     ProfileOwnerId


            public int ProfileOwnerId
            {
                get;
                set;
            }

            //
            // Summary:
            //     Title



            public string Title
            {
                get;
                set;
            }

            //
            // Summary:
            //     FirstName




            public string FirstName
            {
                get;
                set;
            }

            //
            // Summary:
            //     MiddleName



            public string MiddleName
            {
                get;
                set;
            }

            //
            // Summary:
            //     LastName




            public string LastName
            {
                get;
                set;
            }

            //
            // Summary:
            //     Suffix



            public string Suffix
            {
                get;
                set;
            }

            //
            // Summary:
            //     Gender


            public Constants.Gender Gender
            {
                get;
                set;
            }

            //
            // Summary:
            //     BirthDate



            public DateTime BirthDate
            {
                get;
                set;
            }

            //
            // Summary:
            //     InsertId



            public string InsertId
            {
                get;
                set;
            }

            //
            // Summary:
            //     InsertDate


            public DateTime InsertDateTime
            {
                get;
                set;
            }

            //
            // Summary:
            //     UpdateId



            public string UpdateId
            {
                get;
                set;
            }

            //
            // Summary:
            //     UpdateDate


            public DateTime UpdateDateTime
            {
                get;
                set;
            }

            //
            // Summary:
            //     CustomerTypeCode



            public string CustomerTypeCode
            {
                get;
                set;
            }

            //
            // Summary:
            //     Employee Id



            public string EmployeeId
            {
                get;
                set;
            }

            //
            // Summary:
            //     Sida ID



            public string SidaId
            {
                get;
                set;
            }

            //
            // Summary:
            //     Sida expiration date


            public DateTime? SidaExpirationDateTime
            {
                get;
                set;
            }

            //
            // Summary:
            //     LocationCode



            public string LocationCode
            {
                get;
                set;
            }

            //
            // Summary:
            //     UnitedClubMembership


            public bool UnitedClubMembership
            {
                get;
                set;
            }

            //
            // Summary:
            //     ProfileOwnerIndicator


            public bool ProfileOwnerIndicator
            {
                get;
                set;
            }

            //
            // Summary:
            //     Profile Id
            public long ProfileId
            {
                get;
                set;
            }
        public string TravelerTypeCode
        {
            get;
            set;
        }
        public string TravelerTypeDescription { get; set; }
    }
    
}
