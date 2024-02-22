using System;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Common
{
    public interface ICachingService
    {
        Task<string> GetDocument<T>(string key);
        Task<T> GetDocument<T>(string key, string transactionId);
        Task<bool> SaveDocument<T>(string key, T data, long eventTimestamp, TimeSpan SlidingExpiration);
        Task<bool> SaveDocument<T>(string key, T data, string transactionID);
        Task<bool> RemoveDocument(string key, string transactionId);
        Task<string> GetCache<T>(string key, string transactionId);
        Task<bool> SaveCache<T>(string key, T data, string transactionId, TimeSpan expiry);
    }
}
