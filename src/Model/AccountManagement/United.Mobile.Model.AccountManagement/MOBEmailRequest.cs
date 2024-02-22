using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmailRequest : MOBEmpRequest
    {
        private MOBEmail mobEmail;

        public MOBEmail MobEmail
        {
            get
            {
                return mobEmail;
            }
            set
            {
                mobEmail = value;
            }
        }
    }
}
