using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MPStatementResponse : MOBResponse
    {
        private MPStatement statement;

        public MPStatementResponse()
            : base()
        {
        }

        public MPStatement Statement
        {
            get
            {
                return this.statement;
            }
            set
            {
                this.statement = value;
            }
        }
    }
}
