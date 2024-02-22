using System.Xml.Serialization;

namespace United.Mobile.Model.Internal.Exception
{
    public class MOBExceptionWrapper
    {
        private System.Exception exception;

        public MOBExceptionWrapper()
        {
        }

        public MOBExceptionWrapper(System.Exception exception)
        {
            this.exception = exception;
        }
        [XmlElement("ExceptionType")]
        public string ExceptionType { get; set; } = string.Empty;

        [XmlElement("Message")]
        public string Message { get; set; } = string.Empty;

        [XmlElement("Source")]
        public string Source { get; set; } = string.Empty;

        [XmlElement("StackTrace")]
        public string StackTrace { get; set; } = string.Empty;

        [XmlIgnore()]
        public System.Exception Exception { get; set; }
    }
}