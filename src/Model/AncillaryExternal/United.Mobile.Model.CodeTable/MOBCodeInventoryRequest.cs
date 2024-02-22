using United.Mobile.Model.Common;

namespace United.Mobile.Model.CodeTable
{
    public class MOBCodeInventoryRequest : MOBRequest
    {
        public string CodeRequiredFor { get; set; } = string.Empty;

        public MOBCarriersDetailsRequest CarriersDetailsRequest { get; set; }
    }
}
