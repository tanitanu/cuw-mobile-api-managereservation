using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class Phone
    {
        #region Properties

        /// <summary>
        /// Key - combination of Customer Id, Effective date, Channel code, Channel Type code, Channel Type Sequence number
        /// </summary>


        public string Key { get; set; }

        /// <summary>
        /// Phone type
        /// E.g.; H - Home, B - Business, O - Other, U - Unknown
        /// </summary>


        public Constants.PhoneType Type { get; set; }

        /// <summary>
        /// Phone type
        /// E.g.; H - Home, B - Business, O - Other, U - Unknown
        /// </summary>


        public Constants.PhoneType? OldType { get; set; } = null;

        /// <summary>
        /// Phone type Description
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
        /// Country Phone Number 
        /// </summary>



        public string CountryPhoneNumber { get; set; }

        /// <summary>
        /// Country Code
        /// E.g.; US
        /// </summary>




        public string CountryCode { get; set; }

        /// <summary>
        /// AreaCode
        /// </summary>



        public string AreaCode { get; set; }

        /// <summary>
        /// Phone Number
        /// E.g.; 5555551234
        /// </summary>




        public string Number { get; set; }

        /// <summary>
        /// Phone Extension or Pin number
        /// </summary>



        public string ExtensionNumber { get; set; }

        /// <summary>
        /// Label: PH for Phone, WP for Mobile Phone, FX for Fax, P1 for 1-Way Pager, P2 for 2-Way Pager, OT for Other
        /// </summary>


        public Constants.DeviceType DeviceType { get; set; }

        /// <summary>
        /// Primary Phone Flag
        /// True - Phone is the primary contact
        /// </summary>


        public bool PrimaryIndicator { get; set; }

        /// <summary>
        /// IsDefault Flag
        /// True - Phone is the primary contact
        /// </summary>


        public bool IsDefault { get; set; }


        /// <summary>
        /// Day of travel notification
        /// True - Phone number has to be used for communication on day of travel
        /// </summary>


        public bool DayOfTravelNotification { get; set; }

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
        /// Insert flag
        /// </summary>
        //[DataMember(IsRequired = false)]
        //[XmlElement(IsNullable = true)]
        //public bool? IsInsert { get; set; } = false;

        /// <summary>
        /// Private Phone Flag
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
        /// Date from when it will be wrong phone
        /// </summary>


        public DateTime? WrongPhoneDate { get; set; }

        #endregion Properties
    }
}
