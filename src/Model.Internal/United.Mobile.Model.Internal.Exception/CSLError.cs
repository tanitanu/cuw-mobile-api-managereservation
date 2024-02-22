using System;

namespace United.Mobile.Model.Internal.Exception
{
    public class CSLError
    {
        public Error[] Errors { get; set; }

        public DateTime GenerationTime { get; set; }

        public string InnerException { get; set; }

        public string Message { get; set; }

        public string ServerName { get; set; }

        public int Severity { get; set; }

        public string StackTrace { get; set; }

        public string Version { get; set; }
    }

    public class Error
    {
        public string MajorCode { get; set; }

        public string MajorDescription { get; set; }

        public string MinorCode { get; set; }

        public string MinorDescription { get; set; }
    }

}
