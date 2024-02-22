using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.Internal.Common
{
    public class GetRecordsRequest
    {
        [Required]
        public string TableName { get; set; }

        [Required]
        public string[] Keys { get; set; }

        [Required]
        public string TransactionId { get; set; }
    }
}