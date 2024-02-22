using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.SeatMap
{
    public class UIApplication
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsProduction { get; set; }
        public string DeviceId { get; set; }
        public string PushToken { get; set; }
        [Required]
        public UIVersion Version { get; set; }
        public string OsVersion { get; set; }
    }
}
