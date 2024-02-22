using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [DataContract]
    public class ErrorInfo
    {
        //public ErrorInfo();

        [DataMember]
        public string MajorCode { get; set; }
        [DataMember]
        public string MajorDescription { get; set; }
        [DataMember]
        public string MinorCode { get; set; }
        [DataMember]
        public string MinorDescription { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string CallTime { get; set; }
        [DataMember]
        public string VendorCode { get; set; }

        //public ErrorInfo Clone();
    }
}
