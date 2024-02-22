using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Persist;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MPPINPWSecurityQuestionsValidation : IPersist
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Profile.MPPINPWSecurityQuestionsValidation";
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

        public string SessionId { get; set; }

        public int RetryCount { get; set; }

        public List<Securityquestion> SecurityQuestionsFromCSL { get; set; }

        public List<Securityquestion> SecurityQuestionsSentToClient { get; set; }

        public bool AllSecurityQuestionsAnsweredCorrect { get; set; }
    }
}
