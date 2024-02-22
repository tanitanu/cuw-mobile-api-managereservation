#region Assembly United.Service.Presentation.CommonModel, Version=3.9.9.324, Culture=neutral, PublicKeyToken=null
// C:\Account Management Code\Technology\Emerging Technology\United\United 2.0\packages\Presentation.CSL.AnalysisModel.3.9.9.324\lib\net40\United.Service.Presentation.CommonModel.dll
#endregion

using System.Runtime.Serialization;

namespace United.Mobile.Model.Common
{
    [DataContract]
    public class Characteristic : ReferenceDataItem
    {
        public Characteristic() { }

        [DataMember(EmitDefaultValue = false)]
        public Genre Genre { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Code { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Value { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Status Status { get; set; }
    }
}