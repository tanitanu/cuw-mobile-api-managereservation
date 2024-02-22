using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.Payment
{
    public class EmpGenerateHashResponse:EResBaseResponse
    {
        public string HashKey { get; set; }
        public string Kid { get; set; }
        public string Exp { get; set; }
    }
}
