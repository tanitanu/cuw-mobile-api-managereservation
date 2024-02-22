using System;

namespace United.Mobile.Model.CodeTable
{
    [Serializable]
    public class MOBCodeInventoryResponse : MOBResponse
    {
        public MOBCarriersDetailsResponse CarriersDetailsResponse { get; set; }
    }

    
}
