using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class CSLContentMessagesResponse
    {
        public List<CMSContentMessage> Messages { get; set; }
        public List<CMSContentMessageitem> MessageItems { get; set; }
        public string LastCallDateTime { get; set; }
        public string CallTime { get; set; }
        public List<CMSContentError> Errors { get; set; }
        public int Status { get; set; }
    }

    public class CMSContentError
    {
        public string MajorCode { get; set; }
        public string MajorDescription { get; set; }
        public string MinorCode { get; set; }
        public string MinorDescription { get; set; }
        public string Message { get; set; }
        public DateTime CallTime { get; set; }
    }

    [Serializable()]
    public class CMSContentMessage
    {
        public string Title { get; set; }
        public string ContentFull { get; set; }
        public string ContentShort { get; set; }
        public string CallToAction1 { get; set; }
        public string CallToActionUrl1 { get; set; }
        public string Headline { get; set; }
        public List<string> GroupName { get; set; }
        public string LocationCode { get; set; }
    }

    public class CMSContentMessageitem
    {
        public string Title { get; set; }
        public string ContentFull { get; set; }
        public object CallToAction1 { get; set; }
        public object CallToActionURL1 { get; set; }
        public string Headline { get; set; }
        public object ContentText { get; set; }
        public string Vanity { get; set; }
        //public int ID { get; set; }
        public DateTime Created { get; set; }
    }

    [Serializable()]
    public class MOBCSLContentMessagesRequest
    {

        private string lang = string.Empty;
        private string pos = string.Empty;
        private string channel = string.Empty;
        private List<string> listname = new List<string>();
        private string groupname = string.Empty;
        private List<string> locationCodes = new List<string>();
        private bool usecache = false;

        public string Lang
        {
            get
            {
                return lang;
            }
            set
            {
                this.lang = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Pos
        {
            get
            {
                return this.pos;
            }
            set
            {
                this.pos = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Channel
        {
            get
            {
                return channel;
            }
            set
            {
                this.channel = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<string> Listname
        {
            get
            {
                return listname;
            }
            set
            {
                this.listname = value;
            }
        }

        public string Groupname
        {
            get
            {
                return groupname;
            }
            set
            {
                this.groupname = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public List<string> LocationCodes
        {
            get
            {
                return locationCodes;
            }
            set
            {
                this.locationCodes = value;
            }
        }

        public bool Usecache
        {
            get
            {
                return usecache;
            }
            set
            {
                this.usecache = value;
            }
        }

    }
}
