namespace United.Mobile.Model.Common
{
    public class CachingRequest
    {
        public string TransactionId { get; set; }
        public string Key { get; set; }
        public dynamic Data { get; set; }
        //public long EventTimestamp { get; set; }
        public ExpirationOptions ExpirationOptions { get; set; }

    }

    public class CacheRequest
    {
        public string Key { get; set; }
        public string TransactionId
        { get; set; }
    }

    public class SaveCacheRequest
    {
        public string Key { get; set; }
        public dynamic Data { get; set; }
        public string TransactionId { get; set; }
        public ExpirationOptions ExpirationOptions { get; set; }
    }
}
