using System;

namespace United.Mobile.Model.Internal.Common
{
    public class SaveDataRequest<T>
    {
        public string TransactionId { get; set; }
        public string TableName { get; set; }
        public string Key { get; set; }
        public DateTimeOffset AbsoluteExpiration { get; set; } = DateTime.UtcNow.AddDays(2);
        public string SecondaryKey { get; set; }
        public T Data { get; set; }
    }
}