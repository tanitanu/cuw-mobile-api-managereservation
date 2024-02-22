using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class Address
    {
        #region Properties

        /// <summary>
        /// Key - combination of Customer Id, Effective date, Channel code, Channel Type code, Channel Type Sequence number
        /// </summary>


        public string Key { get; set; }

        /// <summary>
        /// Address type
        /// E.g.; H - Home, B - Business, O - Other, U - Unknown
        /// </summary>


        public Constants.AddressType Type { get; set; }


        /// <summary>
        /// Address type
        /// E.g.; H - Home, B - Business, O - Other, U - Unknown
        /// </summary>


        public Constants.AddressType? OldType { get; set; } = null;

        /// <summary>
        /// Address type Description
        /// </summary>

        public string TypeDescription { get; set; }

        /// <summary>
        /// Language that needs to be used for communication
        /// E.g.; en
        /// </summary>


        public string LanguageCode { get; set; }

        /// <summary>
        /// Sequence Number
        /// </summary>

        public int SequenceNumber { get; set; }

        /// <summary>
        /// Address Line 1
        /// E.g.; 233
        /// </summary>



        //[Required(ErrorMessage = "31")]
        public string Line1 { get; set; }

        /// <summary>
        /// Address Line 2
        /// E.g.; South Wacker Drive
        /// </summary>



        public string Line2 { get; set; }

        /// <summary>
        /// Address Line 3
        /// </summary>



        public string Line3 { get; set; }

        /// <summary>
        /// City
        /// E.g.; Chicago
        /// </summary>



        //[Required(ErrorMessage = "36")]
        public string City { get; set; }

        ///// <summary>
        ///// State Code
        ///// E.g.; IL
        ///// </summary>
        //
        //
        //
        //public string State { get; set; }

        /// <summary>
        /// State Code
        /// E.g.; IL
        /// </summary>



        public string StateCode { get; set; }

        /// <summary>
        /// Postal Code
        /// E.g.; 60606
        /// </summary>



        //[Required(ErrorMessage = "38")]
        public string PostalCode { get; set; }

        /// <summary>
        /// CountryCode
        /// E.g.; US
        /// </summary>




        public string CountryCode { get; set; }


        /// <summary>
        ///  CountryNames
        /// </summary>


        public string CountryName { get; set; }

        /// <summary>
        /// Alternate CountryNames
        /// </summary>
        //
        //public string AlternateCountryNames { get; set; }

        /// <summary>
        /// Remark
        /// </summary>



        public string Remark { get; set; }

        /// <summary>
        /// Insert Id
        /// </summary>



        public string InsertId { get; set; }

        /// <summary>
        /// InsertDateTime
        /// </summary>


        public DateTime InsertDateTime { get; set; }

        /// <summary>
        /// Update Id
        /// </summary>



        public string UpdateId { get; set; }

        /// <summary>
        /// UpdateDateTime
        /// </summary>


        public DateTime UpdateDateTime { get; set; }

        /// <summary>
        /// Primary Address Flag
        /// True - Address is the primary contact
        /// </summary>


        public bool PrimaryIndicator { get; set; }

        /// <summary>
        /// Insert flag
        ///// </summary>
        //
        //
        //public bool? IsInsert { get; set; } = false;

        /// <summary>
        /// Private Address Flag
        /// </summary>


        public bool IsPrivate { get; set; }

        /// <summary>
        /// Date from when it will be discontinued
        /// </summary>


        public DateTime? DiscontinuedDate { get; set; }

        /// <summary>
        /// Date from when it will be in effect
        /// </summary>


        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// Date from when it will be wrong address
        /// </summary>


        public string WrongMailDate { get; set; }

        #endregion

    }
}
