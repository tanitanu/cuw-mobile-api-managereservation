using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace United.Mobile.Model.MPSignIn
{
    [DataContract]
    public class BookingType
    {
        [DataMember(EmitDefaultValue = false)]
        public string Display { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string DisplayCode { get; set; }
    }
}
