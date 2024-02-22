#region Assembly United.Service.Presentation.PersonModel, Version=3.9.9.324, Culture=neutral, PublicKeyToken=null
// C:\Account Management Code\Technology\Emerging Technology\United\United 2.0\packages\Presentation.CSL.AnalysisModel.3.9.9.324\lib\net40\United.Service.Presentation.PersonModel.dll
#endregion

using United.Mobile.Model.Common;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using United.Service.Presentation.CommonEnumModel;
using United.Service.Presentation.CommonModel;

namespace United.Mobile.Model.Common
{
    [DataContract]
    public class Document
    {
        public Document() { }

        [DataMember(EmitDefaultValue = false)]
        public virtual string MiddleName { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string Title { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual StateProvince IssueState { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string RedressNumber { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string KnownTravelerNumber { get; set; }
        [DataMember(EmitDefaultValue = true)]
        public virtual DocumentType Type { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual Collection<Country> Nationality { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual Country CountryOfResidence { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string Sex { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string Suffix { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string Surname { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string GivenName { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string DateOfBirth { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string DataSource { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual City IssueCity { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual Country IssueCountry { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual byte[] Image { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual Collection<Characteristic> Characteristic { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string Issuer { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string IssueDate { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string ExpirationDate { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string DocumentID { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string Description { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual Status Status { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual Genre Genre { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual Collection<string> ApisHistory { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual Collection<DocumentDetail> DocumentDetails { get; set; }
    }
}