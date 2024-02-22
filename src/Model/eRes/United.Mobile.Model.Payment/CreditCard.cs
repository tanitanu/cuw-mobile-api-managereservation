using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Payment
{
    public class CreditCard
    {
        public string CVV { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string FirstName { get; set; }
        public string  LastName { get; set; }
        public TypeOption Type{ get; set; }
        public bool IsOAEPPaddingCatalogEnabled { get; set; }
    }

}
