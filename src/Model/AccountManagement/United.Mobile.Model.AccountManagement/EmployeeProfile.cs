using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace United.Mobile.Model.MPSignIn
{
    [DataContract]
    public class EmployeeProfile
    {
        [DataMember(EmitDefaultValue = false)]
        public MOBEmployeeProfileExtended ExtendedProfile { get; set; }
    }
}
