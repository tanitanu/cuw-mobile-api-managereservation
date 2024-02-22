using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBUnitedException : System.Exception
    {
        private string code = string.Empty;

        public string Code
        {
            get; set;
        }
        public MOBUnitedException()
        {
        }

        public MOBUnitedException(string code, string message) : base(message)
        {
            this.Code = code;
            //base.Message = message;
        }

        public MOBUnitedException(string message)
            : base(message)
        {
        }

        public MOBUnitedException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        protected MOBUnitedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }
}
