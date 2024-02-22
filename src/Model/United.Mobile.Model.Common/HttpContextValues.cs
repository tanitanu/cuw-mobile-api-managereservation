using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model
{
    public class HttpContextValues
    {
        public Application Application { get; set; }

        [Required]
        public string DeviceId { get; set; } = string.Empty;

        [Required]
        public string LangCode { get; set; } = string.Empty;

        [Required]
        public string RequestTimeUtc { get; set; } = string.Empty;

        [Required]
        public string TransactionId { get; set; } = string.Empty;

        public string SessionId { get; set; } = string.Empty;
    }
}