#region Assembly United.Service.Presentation.PersonModel, Version=3.9.9.324, Culture=neutral, PublicKeyToken=null
// C:\Account Management Code\Technology\Emerging Technology\United\United 2.0\packages\Presentation.CSL.AnalysisModel.3.9.9.324\lib\net40\United.Service.Presentation.PersonModel.dll
#endregion

using System.Runtime.Serialization;
using United.Service.Presentation.CommonEnumModel;
using United.Service.Presentation.CommonModel;

namespace United.Mobile.Model.Common
{
    [DataContract]
    public class DocumentDetail
    {
        public DocumentDetail() { }

        [DataMember(EmitDefaultValue = false)]
        public virtual string DocumentID { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string ExpirationDate { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual Country IssueCountry { get; set; }
        [DataMember(EmitDefaultValue = true)]
        public virtual DocumentType Type { get; set; }
    }
}