using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MPAccountValidationResponse : MOBResponse
    {
        private MPAccountValidation accountValidation;

        public MPAccountValidationResponse()
            : base()
        {
        }

        public MPAccountValidation AccountValidation 
        { 
            get
            {
                return this.accountValidation;
            }
            set
            {
                this.accountValidation = value;
            }
        }
    }
}
