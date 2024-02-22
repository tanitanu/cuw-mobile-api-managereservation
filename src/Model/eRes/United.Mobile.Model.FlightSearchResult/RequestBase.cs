using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.FlightSearchResult
{
    public class RequestBase
    {
        [Required]
        public string SessionId { get; set; }
    }
}
