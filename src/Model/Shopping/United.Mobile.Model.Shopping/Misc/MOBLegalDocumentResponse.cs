using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class LegalDocumentResponse : MOBResponse
    {
        private List<MOBTypeOption> legalDocuments;

        public List<MOBTypeOption> LegalDocuments
        {
            get
            {
                return this.legalDocuments;
            }
            set
            {
                this.legalDocuments = value;
            }
        }
    }
}
