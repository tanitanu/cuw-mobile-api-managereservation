using System.Runtime.Serialization;

namespace United.Mobile.Model.TripPlannerService
{
    [DataContract]
    public class Version
    {
        [DataMember]
        public int Major { get; set; }
        [DataMember]
        public int? Minor { get; set; }
        [DataMember]
        public int Build { get; set; }

        //public override string ToString()
        //{
        //    return $"{Major}.{Minor}.{Build}";
        //}
    }
}
