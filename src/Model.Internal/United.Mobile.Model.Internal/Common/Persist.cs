using System;

namespace United.Mobile.Model.Internal.Common
{
    public class Persist<T>
    {
        public string Key { get; set; }
        public T Data { get; set; }
        public long EventTimestamp { get; set; }
        public DateTime DocumentSavedTimeUtc { get; set; }
        public TimeSpan SlidingExpiration { get; set; }
        public DateTime ExpirationDateTimeUtc { get; set; }

    }
}
