using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.MPSignIn
{
    public class CslResponse<T>
    {
        public dynamic Meta { get; set; }
        public T Data { get; set; }
        public dynamic Link { get; set; }
       // public IEnumerable<Error> Errors { get; set; }

    }
}
