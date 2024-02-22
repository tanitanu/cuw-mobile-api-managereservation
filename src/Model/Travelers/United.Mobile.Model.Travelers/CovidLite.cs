using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace United.Mobile.Model.Travelers
{
    [DataContract]
    public class Flight
    {
        [DataMember]
        public string Origin { get; set; }

        [DataMember]
        public string Destination { get; set; }

        [DataMember]
        public string DepartureDateTimeUTC { get; set; }

        [DataMember]
        public string ArrivalDateTimeUTC { get; set; }

        [DataMember]
        public string OperatorCarrierCode { get; set; }

        [DataMember]
        public int OperatorFlightNumber { get; set; }
    }

    [DataContract]
    public class SimpleResponse
    {
        [DataMember]
        public Int32 ErrCode = 0;

        [DataMember]
        public string ErrMsg = string.Empty;
    }

    [DataContract]
    public class CovidLiteRequest
    {
        [DataMember]
        public Collection<Flight> Flights { get; set; }

        [DataMember]
        public bool TestFlightOnly { get; set; }
    }

    [DataContract]
    public class CovidLiteResponse : SimpleResponse
    {
        [DataMember]
        public Collection<CovidLite> CovidLites { get; set; }
    }

    [DataContract]
    public class CovidLite
    {
        [DataMember(EmitDefaultValue = false)]
        public Int32 CovidId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Collection<string> TestTypeList { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Collection<string> TestTerminologyList { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string IssueOrTaken { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Destination { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string TestIssueDate { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public bool IsOverrideRequired { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string TestPeriodUnit { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ValidityPeriod { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ValidityPeriodHtml { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string HeaderInformation { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string Information { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public string InformationURL { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string TestPeriodFrom { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TestPeriod { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ExceptionNationalities { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public bool IsTestFlight { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public bool IsTransit { get; set; }
    }
}
