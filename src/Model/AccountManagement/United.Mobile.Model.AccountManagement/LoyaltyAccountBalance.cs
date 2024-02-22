#region Assembly United.Service.Presentation.CommonModel, Version=3.9.9.324, Culture=neutral, PublicKeyToken=null
// C:\Account Management Code\Technology\Emerging Technology\United\United 2.0\packages\Presentation.CSL.AnalysisModel.3.9.9.324\lib\net40\United.Service.Presentation.CommonModel.dll
#endregion

using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using United.Service.Presentation.CommonEnumModel;

namespace United.Mobile.Model.Common
{
    [DataContract]
    public class LoyaltyAccountBalance
    {
        public LoyaltyAccountBalance() { }

        [DataMember(EmitDefaultValue = false)]
        public virtual int Balance { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual Collection<string> Descriptions { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string ExpirationDate { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string EffectiveDate { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string TravelBy { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string BatchDescription { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string BatchID { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual Genre Type { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string FormID { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual Collection<Characteristic> Characteristics { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual LoyaltyAccountBalanceType BalanceType { get; set; }
    }
}