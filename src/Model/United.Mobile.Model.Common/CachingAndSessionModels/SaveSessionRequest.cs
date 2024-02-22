using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.Common
{
    public class SaveSessionRequest
    {
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public string SessionId { get; set; }
        [Required]
        public string ObjectName { get; set; }
        [Required]
        public dynamic Data { get; set; }
        public ICollection<string> ValidationParams { get; set; }
        public ExpirationOptions ExpirationOptions { get; set; }
    }
}
