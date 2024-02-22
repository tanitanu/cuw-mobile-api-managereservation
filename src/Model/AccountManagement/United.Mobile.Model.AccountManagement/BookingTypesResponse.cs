using System.Collections.Generic;
using System.Runtime.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.MPSignIn
{
    [DataContract]
    public class BookingTypesResponse : MOBResponse
    {

        [DataMember(EmitDefaultValue = false)]
        public EmployeeJA EmployeeJA { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public EmployeeJA LoggedInJA { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool AllowImpersonation { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ImpersonateType { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public PassRiderExtended LoggedInPassRider { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public EmployeeProfile EmployeeProfile { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<BookingType> BookingTypes { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<BookingType> QualifiedEmergencyTypes { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<BookingType> EmergencyNatureTypes { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool IsTermsAndConditionsAccepted { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int NumberOfPassengersInJA { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string PositiveSpaceAlertMessage { get; set; }
    }
}
