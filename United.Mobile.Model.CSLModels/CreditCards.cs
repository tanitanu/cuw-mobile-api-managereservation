using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class CreditCards:RequestBase
    {
            //
            // Summary:
            //     Id

            public string _id
            {
                get;
                set;
            }

            //
            // Summary:
            //     Type - VI, MC

            public string Type
            {
                get;
                set;
            }

            //
            // Summary:
            //     PrimayIndicator

            public bool PrimayIndicator
            {
                get;
                set;
            }

            //
            // Summary:
            //     DefaultIndicator

            public bool DefaultIndicator
            {
                get;
                set;
            }

            //
            // Summary:
            //     PriorityNumber

            public int PriorityNumber
            {
                get;
                set;
            }

            //
            // Summary:
            //     Remark

            public string Remark
            {
                get;
                set;
            }

            //
            // Summary:
            //     PhoneContactTypeId

            public string PhoneContactTypeId
            {
                get;
                set;
            }

            //
            // Summary:
            //     AddressContactTypeId

            public string AddressContactTypeId
            {
                get;
                set;
            }

            //
            // Summary:
            //     Name

            public string Name
            {
                get;
                set;
            }

            //
            // Summary:
            //     ExpiryMonth

            public int ExpiryMonth
            {
                get;
                set;
            }

            //
            // Summary:
            //     ExpiryYear

            public int ExpiryYear
            {
                get;
                set;
            }

            //
            // Summary:
            //     RSAEncryptedCard

            public string RSAEncryptedCard
            {
                get;
                set;
            }

            //
            // Summary:
            //     RSAEncryptedHMACCard

            public string RSAEncryptedHMACCard
            {
                get;
                set;
            }

            //
            // Summary:
            //     Token

            public string Token
            {
                get;
                set;
            }

            //
            // Summary:
            //     Insert Id

            public string InsertId
            {
                get;
                set;
            }

            //
            // Summary:
            //     InsertDateTime

            public DateTime InsertDateTime
            {
                get;
                set;
            }

            //
            // Summary:
            //     Update Id

            public string UpdateId
            {
                get;
                set;
            }

            //
            // Summary:
            //     UpdateDateTime

            public DateTime UpdateDateTime
            {
                get;
                set;
            }
        }    
}
