using System;

namespace United.Mobile.Model.Common
{
    public class ExpirationOptions
    {
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
    }
}