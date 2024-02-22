using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class UpdateContactResponse : Base
    {
        /// <summary>
        /// AddressKey - combination of Customer Id, Effective date, Channel code, Channel Type code, Channel Type Sequence number
        /// </summary>


        public string AddressKey { get; set; }

        /// <summary>
        /// UpdateDateTime for address
        /// </summary>


        public DateTime? AddressUpdateDateTime { get; set; }

        /// <summary>
        /// PhoneKey - combination of Customer Id, Effective date, Channel code, Channel Type code, Channel Type Sequence number
        /// </summary>


        public string PhoneKey { get; set; }


        /// <summary>
        /// UpdateDateTime for phone
        /// </summary>


        public DateTime? PhoneUpdateDateTime { get; set; }

        /// <summary>
        /// Email Key - combination of Customer Id, Effective date, Channel code, Channel Type code, Channel Type Sequence number
        /// </summary>


        public string EmailKey { get; set; }

        /// <summary>
        /// UpdateDateTime for email
        /// </summary>


        public DateTime? EmailUpdateDateTime { get; set; }
    }
}
