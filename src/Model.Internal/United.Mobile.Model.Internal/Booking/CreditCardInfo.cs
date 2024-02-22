using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.Booking
{
    public class CreditCardInfo
    {
        public List<string> CreditCardMonth { get; set; } = new List<string>();
        public List<string> CreditCardYear { get; set; } = new List<string>();
        public List<TypeOption> CreditCardTypes { get; set; } = new List<TypeOption>();
    }
}