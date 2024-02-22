using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBMPPINPWDSecurityItems
    {
        public MOBMPPINPWDSecurityItems()
            : base()
        {
            needQuestionsCount = 0; //TODO Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfSecurityQuestionsNeedatPINPWDUpdate"].ToString());
        }
        private string primaryEmailAddress;
        public string PrimaryEmailAddress
        {
            get
            {
                return this.primaryEmailAddress;
            }
            set
            {
                this.primaryEmailAddress = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        private int needQuestionsCount;
        public int NeedQuestionsCount
        {
            get
            {
                return this.needQuestionsCount;
            }
            set
            {
                this.needQuestionsCount = value;
            }
        }

        private List<Securityquestion> securityQuestions;
        public List<Securityquestion> AllSecurityQuestions
        {
            get { return securityQuestions; }
            set { securityQuestions = value; }
        }

        private List<Securityquestion> savedSecurityQuestions;
        public List<Securityquestion> SavedSecurityQuestions
        {
            get { return savedSecurityQuestions; }
            set { savedSecurityQuestions = value; }
        }

        private string updatedPassword;
        public string UpdatedPassword
        {
            get
            {
                return this.updatedPassword;
            }
            set
            {
                this.updatedPassword = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

    }
}
