using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class SecureTravelerInformation
    {
            //
            // Summary:
            //     Document Id
            public string _id
            {
                get;
                set;
            }

            //
            // Summary:
            //     Customer Id


            public long CustomerId
            {
                get;
                set;
            }

            //
            // Summary:
            //     Loyalty Id


            public string LoyaltyId
            {
                get;
                set;
            }

            //
            // Summary:
            //     Secure Traveler Sequence Number



            public int SecureTravelerSeqNum
            {
                get;
                set;
            }

            //
            // Summary:
            //     Firstname



            public string FirstName
            {
                get;
                set;
            }

            //
            // Summary:
            //     Lastname



            public string LastName
            {
                get;
                set;
            }

            //
            // Summary:
            //     Middle name

            public string MiddleName
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



            public string Description
            {
                get;
                set;
            }



            public string DocumentType
            {
                get;
                set;
            }

            //
            // Summary:
            //     Suplementary Travel Number Type


            public char SupTravelNumberType
            {
                get;
                set;
            }

            //
            // Summary:
            //     Suplementary Travel Info


            public string SupTravelInfo
            {
                get;
                set;
            }



            public string BirthPlace
            {
                get;
                set;
            }



            public string IssuePlace
            {
                get;
                set;
            }



            public DateTime IssueDate
            {
                get;
                set;
            }

            //
            // Summary:
            //     CountryCode



            public string CountryCode
            {
                get;
                set;
            }

            //
            // Summary:
            //     Insert id


            public string InsertId
            {
                get;
                set;
            }

            //
            // Summary:
            //     Update id


            public string UpdateId
            {
                get;
                set;
            }

            //
            // Summary:
            //     Insert date


            public DateTime InsertDate
            {
                get;
                set;
            }

            //
            // Summary:
            //     Update date


            public DateTime UpdateDate
            {
                get;
                set;
            }
        }
    
}
