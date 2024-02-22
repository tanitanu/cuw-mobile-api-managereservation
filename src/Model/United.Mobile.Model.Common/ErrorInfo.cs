using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace United.Mobile.Model.Common
{
    //[DataContract]
    public class ErrorInfo
    {
        //public ErrorInfo();
        public int? ErrorCode { get; set; } 
        public string ErrorDescription { get; set; } 
        public DateTime CallTime { get; set; } = DateTime.Now;

    }
}
