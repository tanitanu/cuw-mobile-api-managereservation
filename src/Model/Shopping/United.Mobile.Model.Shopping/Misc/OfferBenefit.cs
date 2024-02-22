using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class OfferBenefit
    {
        private string type = string.Empty;
        private List<string> headers;
        private List<string> contents;

        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public List<string> Headers
        {
            get
            {
                return this.headers;
            }
            set
            {
                this.headers = value;
            }
        }

        public List<string> Contents
        {
            get
            {
                return this.contents;
            }
            set
            {
                this.contents = value;
            }
        }
    }
}
