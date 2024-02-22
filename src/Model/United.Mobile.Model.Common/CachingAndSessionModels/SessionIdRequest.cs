using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.Common
{
    public class SessionIdRequest
    {
        [Required]
        public string TransactionId { get; set; }
        public ICollection<string> ValidationParams { get; set; }
        public ExpirationOptions ExpirationOptions { get; set; }
    }
}
