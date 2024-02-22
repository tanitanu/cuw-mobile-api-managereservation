using System;

namespace United.Mobile.Model.Internal.Common
{
    public class ErrorInfo
    {       
        public int? ErrorCode { get; set; }
        public string ErrorDescription { get; set; } 
        public DateTime CallTime { get; set; } = DateTime.Now;
    }
}