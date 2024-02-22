using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    public class TnC
    {
        private string type = string.Empty;
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

        public List<string> Headers { get; set; } 
        public List<string> Contents { get; set; }
        public TnC()
        {
            Headers = new List<string>();
            Contents = new List<string>();
        }
    }
}
