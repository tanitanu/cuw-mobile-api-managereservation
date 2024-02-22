using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class SupplementaryTravelDocsDataMembers: SupplementaryTravelDocsSharedMembers
    {
        public bool IsNew
        {
            get;
            set;
        }



        public bool IsSelected
        {
            get;
            set;
        }



        public long CustomerId
        {
            get;
            set;
        }




        public string InsertId
        {
            get;
            set;
        }



        public DateTime? InsertDtmz
        {
            get;
            set;
        }




        public string UpdateId
        {
            get;
            set;
        }



        public DateTime? UpdateDtmz
        {
            get;
            set;
        }
    }
    public class SupplementaryTravelDocsSharedMembers
    {



        public string CountryCode
        {
            get;
            set;
        }



        public DateTime? IssueDate
        {
            get;
            set;
        }




        public string Number
        {
            get;
            set;
        }



        public string PlaceOfBirth
        {
            get;
            set;
        }




        public string PlaceOfIssue
        {
            get;
            set;
        }



        public decimal SeqNumber
        {
            get;
            set;
        }




        public string Type
        {
            get;
            set;
        }
    }
}
