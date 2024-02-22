using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace United.Mobile.Model.Internal.Exception
{
    [Serializable()]
    public class MOBUnitedException : System.Exception
    {

        public string Code { get; set; } = string.Empty;

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