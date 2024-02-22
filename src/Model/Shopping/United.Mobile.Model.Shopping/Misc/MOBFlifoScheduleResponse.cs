using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlifoScheduleResponse 
    {
        public string CallTime= string.Empty;

        public FlifoScheduleError[] Errors;

        public string Message = string.Empty;

        public FlifoScheduleTrip[] Schedule;

        public string ServerName = string.Empty;

        public string Status = string.Empty;

 
        ///// <remarks/>
        //public string CallTime
        //{
        //    get
        //    {
        //        return this.CallTime;
        //    }
        //    set
        //    {
        //        this.CallTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        //    }
        //}

        ////[System.Xml.Serialization.XmlArrayAttribute(IsNullable = true)]
        //public FlifoScheduleError[] Errors
        //{
        //    get
        //    {
        //        return this.errorsField;
        //    }
        //    set
        //    {
        //        this.errorsField = value;
        //    }
        //}

        ///// <remarks/>
        ////[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        //public string Message
        //{
        //    get
        //    {
        //        return this.messageField;
        //    }
        //    set
        //    {
        //        this.messageField = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        //    }
        //}

        ///// <remarks/>
        ////[System.Xml.Serialization.XmlArrayAttribute(IsNullable = true)]
        //public FlifoScheduleTrip[] Schedule
        //{
        //    get
        //    {
        //        return this.scheduleField;
        //    }
        //    set
        //    {
        //        this.scheduleField = value;
        //    }
        //}

        ////[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        //public string ServerName
        //{
        //    get
        //    {
        //        return this.serverNameField;
        //    }
        //    set
        //    {
        //        this.serverNameField = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        //    }
        //}

        ////[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        //public string Status
        //{
        //    get
        //    {
        //        return this.statusField;
        //    }
        //    set
        //    {
        //        this.statusField = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        //    }
        //}
    }
}
