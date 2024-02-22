using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBBKAddTravellerRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string pax1Id = string.Empty; 
        private string pax2Id = string.Empty; 
        private string pax3Id = string.Empty; 
        private string pax4Id = string.Empty; 
        private string pax5Id = string.Empty; 
        private string pax6Id = string.Empty; 
        private string pax7Id = string.Empty; 
        private string pax8Id = string.Empty; 
        private string pax1Type = string.Empty; 
        private string pax2Type = string.Empty; 
        private string pax3Type = string.Empty; 
        private string pax4Type = string.Empty; 
        private string pax5Type = string.Empty; 
        private string pax6Type = string.Empty; 
        private string pax7Type = string.Empty; 
        private string pax8Type = string.Empty;
        private string mpAccountNumber = string.Empty;

        public string Pax1Id
        {
            get
            {
                return this.pax1Id;
            }
            set
            {
                this.pax1Id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax2Id
        {
            get
            {
                return this.pax2Id;
            }
            set
            {
                this.pax2Id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax3Id
        {
            get
            {
                return this.pax3Id;
            }
            set
            {
                this.pax3Id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax4Id
        {
            get
            {
                return this.pax4Id;
            }
            set
            {
                this.pax4Id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax5Id
        {
            get
            {
                return this.pax5Id;
            }
            set
            {
                this.pax5Id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax6Id
        {
            get
            {
                return this.pax6Id;
            }
            set
            {
                this.pax6Id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax7Id
        {
            get
            {
                return this.pax7Id;
            }
            set
            {
                this.pax7Id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax8Id
        {
            get
            {
                return this.pax8Id;
            }
            set
            {
                this.pax8Id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax1Type
        {
            get
            {
                return this.pax1Type;
            }
            set
            {
                this.pax1Type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax2Type
        {
            get
            {
                return this.pax2Type;
            }
            set
            {
                this.pax2Type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax3Type
        {
            get
            {
                return this.pax3Type;
            }
            set
            {
                this.pax3Type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax4Type
        {
            get
            {
                return this.pax4Type;
            }
            set
            {
                this.pax4Type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax5Type
        {
            get
            {
                return this.pax5Type;
            }
            set
            {
                this.pax5Type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax6Type
        {
            get
            {
                return this.pax6Type;
            }
            set
            {
                this.pax6Type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax7Type
        {
            get
            {
                return this.pax7Type;
            }
            set
            {
                this.pax7Type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Pax8Type
        {
            get
            {
                return this.pax8Type;
            }
            set
            {
                this.pax8Type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string MPAccountNumber
        {
            get
            {
                return this.mpAccountNumber;
            }
            set
            {
                this.mpAccountNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
