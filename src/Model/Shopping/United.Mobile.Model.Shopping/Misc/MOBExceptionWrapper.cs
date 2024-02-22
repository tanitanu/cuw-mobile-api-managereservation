using System;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    [XmlRoot("ExceptionInfo")]
    public class ExceptionWrapper
    {
        private System.Exception exception;

        public ExceptionWrapper()
        {
        }

        public ExceptionWrapper(System.Exception exception)
        {
            this.exception = exception;
        }

        [XmlElement("ExceptionType")]
        public string ExceptionType
        {
            get
            {
                return this.exception.GetType().AssemblyQualifiedName;
            }
            set
            {
            }
        }

        [XmlElement("Message")]
        public string Message
        {
            get
            {
                return this.exception.Message;
            }
            set
            {
            }
        }

        [XmlElement("Source")]
        public string Source
        {
            get
            {
                return this.exception.Source;
            }
            set
            {
            }
        }

        [XmlElement("StackTrace")]
        public string StackTrace
        {
            get
            {
                return this.exception.StackTrace;
            }
            set
            {
            }
        }

        [XmlIgnore()]
        public System.Exception Exception
        {
            get
            {
                return this.exception;
            }
            set
            {
                this.exception = value;
            }
        }
    }
}
