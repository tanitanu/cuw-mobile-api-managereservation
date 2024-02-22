using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class CreditCardDataReponseModel : Base
    {

        public List<ProfileCreditCardItem> CreditCards { get; set; }

        public bool? hasPrimaryCard { get; set; }

        public string ExpirationDate { get; set; }

    }
    public class ProfileCreditCardItem
    {


        public bool IsNew { get; set; }


        public bool IsSelected { get; set; }


        public bool IsDefault { get; set; }


        public int ExpYear { get; set; }


        public int ExpMonth { get; set; }


        public string UpdateId { get; set; }


        public DateTime? UpdateDtmz { get; set; }


        public bool IsPrimary { get; set; }



        public DateTime? InsertDtmz { get; set; }


        public string Key { get; set; }


        public string PhoneKey { get; set; }


        public string LanguageCode { get; set; }



        public string AddressKey { get; set; }



        public string AddressChannelCode { get; set; }


        public string AddressChannelTypeCode { get; set; }



        public int? AddressChannelTypeSeqNum { get; set; }



        public DateTime? AddressEffectiveDate { get; set; }


        public string InsertId { get; set; }



        public int? PhoneChannelTypeSeqNum { get; set; }


        public long? CustomerId { get; set; }


        public string MileagePlusId { get; set; }


        public string CCTypeDescription { get; set; }


        public string CustomDescription { get; set; }


        public string PhoneChannelCode { get; set; }


        public string PhoneChannelTypeCode { get; set; }


        public DateTime? PhoneEffectiveDate { get; set; }


        public string Code { get; set; }


        public Person Payor { get; set; }


        public string PersistentToken { get; set; }


        public string ExpirationDate { get; set; }


        public string AccountNumberMasked { get; set; }


        public string AccountNumberToken { get; set; }


        public string AccountNumberLastFourDigits { get; set; }


    }

}
