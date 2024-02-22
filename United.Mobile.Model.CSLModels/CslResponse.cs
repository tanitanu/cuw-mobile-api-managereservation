using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class CslResponse<T>
    {
        public dynamic Meta { get; set; }
        public T Data { get; set; }
        public dynamic Link { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }
}
