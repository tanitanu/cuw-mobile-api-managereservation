using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.Payment
{
    public class EmpCreditCard
    {
        public string CID { get; set; }
        public string ExpMonth { get; set; }
        public string ExpYear { get; set; }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public TypeOption Type{ get; set; }
    }
}
