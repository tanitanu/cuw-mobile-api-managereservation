using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.Common
{
    public class SessionRequest : SessionIdRequest
    {
        [Required]
        public string SessionId { get; set; }
        public string ObjectName { get; set; }
    }
}