namespace United.Mobile.Model.Internal.Exception
{
    public class MOBException
    {
        public string Code { get; set; } = string.Empty;
        private string _message;
        public string Message { get=> _message; set=> _message= string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        public string ErrMessage { get; set; } = string.Empty;
        
        public MOBException()
        {
        }
        public MOBException(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}