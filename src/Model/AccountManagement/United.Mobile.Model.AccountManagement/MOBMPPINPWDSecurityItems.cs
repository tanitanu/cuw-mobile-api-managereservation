using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBMPPINPWDSecurityItems
    {
        public MOBMPPINPWDSecurityItems()
        {
                
        }
        public MOBMPPINPWDSecurityItems(IConfiguration configuration)
            : base()
        {
            needQuestionsCount = configuration.GetValue<int>("NumberOfSecurityQuestionsNeedatPINPWDUpdate"); 
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

        public int needQuestionsCount { get; set; }
       

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
