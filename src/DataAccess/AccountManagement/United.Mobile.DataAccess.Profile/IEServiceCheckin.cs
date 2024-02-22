using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Profile
{
    public interface IEServiceCheckin
    {
        Task<T> GetPhoneValidation<T>(string token, string urlPath, string requestData, string sessionId);
    }
}
