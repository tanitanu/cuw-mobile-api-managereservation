using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace United.Utility.HttpService
{
    public interface IShoppingClientService
    {
        public Task<T> PostAsync<T>(string token, string sessionId, string action, object request, string contentType = "application/json");
        public Task<T> PostAsyncForReShop<T>(string token, string sessionId, string action, object request, string contentType = "application/json");
    }
}
