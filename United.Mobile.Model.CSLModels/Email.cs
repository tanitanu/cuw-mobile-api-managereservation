using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class Email
    {
        /// <summary>
        /// Key - combination of Customer Id, Effective date, Channel code, Channel Type code, Channel Type Sequence number
        /// </summary>


        public string Key { get; set; }

        /// <summary>
        /// Email type
        /// E.g.; H - Home, B - Business, O - Other, U - Unknown
        /// </summary>


        public Constants.EmailType Type { get; set; }

        /// <summary>
        /// Email type
        /// E.g.; H - Home, B - Business, O - Other, U - Unknown
        /// </summary>


        public Constants.EmailType? OldType { get; set; } = null;

        /// <summary>
        /// Email type Description
        /// </summary>

        public string TypeDescription { get; set; }

        /// <summary>
        /// Sequence Number
        /// </summary>

        public int SequenceNumber { get; set; }

        /// <summary>
        /// Email Address
        /// </summary>




        public string Address { get; set; }

        /// <summary>
        /// Additional Remark
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
        /// Primary Email Flag
        /// True - Email is the primary contact
        /// </summary>


        public bool PrimaryIndicator { get; set; }

        /// <summary>
        /// Day of travel notification
        /// True - Email address has to be used for communication on day of travel
        /// </summary>


        public bool DayOfTravelNotification { get; set; }

        /// <summary>
        /// Language that needs to be used for communication
        /// E.g.; en
        /// </summary>



        public string LanguageCode { get; set; }

        /// <summary>
        /// Insert flag
        ///// </summary>
        //[DataMember(IsRequired = false)]
        //[XmlElement(IsNullable = true)]
        //public bool? IsInsert { get; set; } = false;

        /// <summary>
        /// Private Email Flag
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
        /// Date from when it will be wrong email
        /// </summary>


        public DateTime? WrongEmailDate { get; set; }
    }
}
