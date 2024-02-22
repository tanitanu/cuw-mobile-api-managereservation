using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CCEPromo //: IPersist
    {
        #region IPersist Members

        private string objectName = "United.Mobile.CCE.Service.Presentation.Personalization";
        public string ObjectName
        {
            get
            {
                return this.objectName;
            }
            set
            {
                this.objectName = value;
            }
        }

        #endregion
   
        private United.Service.Presentation.PersonalizationRequestModel.ContextualCommRequest contextualCommRequest;

        private string contextualCommResponseJson;

        public string ContextualCommResponseJson
        {
            get { return contextualCommResponseJson; }
            set { contextualCommResponseJson = value; }
        }

        public United.Service.Presentation.PersonalizationRequestModel.ContextualCommRequest ContextualCommRequest
        {
            get { return contextualCommRequest; }
            set { contextualCommRequest = value; }
        }

    }
}
