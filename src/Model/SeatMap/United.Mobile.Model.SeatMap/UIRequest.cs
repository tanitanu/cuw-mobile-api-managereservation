using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.SeatMap
{
    public class UIRequest<T> : IUIRequest<T>
    {
        [Required]
        public UIApplication Application { get; set; }

        [Required]
        public string DeviceId { get; set; }

        [Required]
        public string TransactionId { get; set; }
        
        [Required]
        public T Data { get; set; }

        public string LanguageCode { get; set; }

        public string PushToken { get; set; }

        public string SessionId { get; set; }
    }

    public interface IUIRequest<T>
    {
        UIApplication Application { get; set; }
        
        string DeviceId { get; set; }
        
        string TransactionId { get; set; }
        
        T Data { get; set; }
        
        string LanguageCode { get; set; }
        
        string PushToken { get; set; }
    }
}
