using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ComBookingClass
    {
        private ComCabin cabin;
        private string code = string.Empty;

        public ComCabin Cabin
        {
            get
            {
                return this.cabin;
            }
            set
            {
                this.cabin = value;
            }
        }

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
