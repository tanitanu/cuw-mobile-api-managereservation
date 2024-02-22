using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpPassType
    {
        private string name;
        private string code;
        //Add description to Passtype.
        //ALM 27545 - Mobile16C Release - 21June2016
        private string description;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
