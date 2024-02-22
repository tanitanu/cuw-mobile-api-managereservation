﻿#region Assembly United.Service.Presentation.CommonModel, Version=3.9.9.324, Culture=neutral, PublicKeyToken=null
// C:\Account Management Code\Technology\Emerging Technology\United\United 2.0\packages\Presentation.CSL.AnalysisModel.3.9.9.324\lib\net40\United.Service.Presentation.CommonModel.dll
#endregion

using System.Runtime.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Common
{
    [DataContract]
    public class Status
    {
        public Status() { }

        [DataMember(EmitDefaultValue = false)]
        public string DefaultIndicator { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int DisplaySequence { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Key { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string Code { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual Genre Type { get; set; }
    }
}