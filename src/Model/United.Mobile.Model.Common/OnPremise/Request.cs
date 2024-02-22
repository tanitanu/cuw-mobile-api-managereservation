using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.Common.OnPremise
{
    public class Request<T>
    {
        [Required]
        public Application Application { get; set; }
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public T Data { get; set; }
        [Required]
        public string LanguageCode { get; set; }
        public string DeviceId { get; set; }
        public string PushToken { get; set; }
        public string AccessCode { get; set; }
    }
}