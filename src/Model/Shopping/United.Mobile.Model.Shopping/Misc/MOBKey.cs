using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class MOBKey
    {
        private int index;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

       

        public string key { get; set; } = string.Empty;
       

        private string val;

        public string Val
        {
            get { return val; }
            set { val = value; }
        }

    }
}
