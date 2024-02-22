using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CodeTableResponse : MOBResponse
    {
        private List<MOBCountry> countries;

        public CodeTableResponse()
            : base()
        {
        }

        public List<MOBCountry> Countries
        {
            get
            {
                return this.countries;
            }
            set
            {
                this.countries = value;
            }
        }
    }
}
