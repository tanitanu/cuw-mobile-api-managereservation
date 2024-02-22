using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.BagCalculator
{
    [Serializable()]
    public class ClassOfService
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ClassOfService(string classOfServiceCode, string desc)
        {
            Code = classOfServiceCode;
            Description = desc;
        }
    }
}
